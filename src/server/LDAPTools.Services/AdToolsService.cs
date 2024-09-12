using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Runtime.Versioning;
using LDAPTools.Models;
using Microsoft.Extensions.Configuration;

[assembly: SupportedOSPlatform("windows")]

namespace LDAPTools.Services
{
    public class AdToolsService
    {
        private readonly string _domain;

        public AdToolsService(IConfiguration configuration)
        {
            _domain = configuration["ActiveDirectory:Domain"] 
                      ?? throw new InvalidOperationException("Active Directory domain must be specified in the configuration.");
        }

        private PrincipalContext CreatePrincipalContext()
        {
            return new PrincipalContext(ContextType.Domain, _domain);
        }

        // Get all users with optional filtering
        public List<LdapUser> GetAdUsers(string? query = null, int? start = null, int? end = null)
        {
            using var context = CreatePrincipalContext();
            UserPrincipal principal = new(context)
            {
                Enabled = true,
                SamAccountName = string.IsNullOrEmpty(query) ? null : $"{query}*"
            };

            return SearchPrincipals(principal, start, end)
                .Select(user => new LdapUser(user))
                .ToList();
        }

        // Add a new user to AD
        public LdapUser AddAdUser(LdapUser user)
        {
            using var context = CreatePrincipalContext();
            var newUser = new UserPrincipal(context)
            {
                SamAccountName = user.SamAccountName,
                EmailAddress = user.EmailAddress,
                DisplayName = user.DisplayName,
                Enabled = true
            };
            //newUser.SetPassword(user.Password);
            newUser.Save();

            return new LdapUser(newUser);
        }

        // Get a single user by username
        public LdapUser GetAdUser(string username)
        {
            using var context = CreatePrincipalContext();
            return new LdapUser(GetUserPrincipal(context, username));
        }

        // Check if a user exists
        public bool AdUserExists(string username)
        {
            using var context = CreatePrincipalContext();
            return GetUserPrincipal(context, username) != null;
        }

        // Check if a user is a member of a specific group
        public bool IsAdUserInGroup(string username, string groupName)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            return user.GetGroups()
                       .Cast<GroupPrincipal>()
                       .Any(g => g.Name == groupName);
        }

        // Authenticate a user with username and password
        public bool AdAuthenticateUser(string username, string password)
        {
            using var context = new PrincipalContext(ContextType.Domain, _domain);
            return context.ValidateCredentials(username, password);
        }

        // Change a user's password
        public bool ChangeAdUserPassword(string username, string newPassword)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            user.SetPassword(newPassword);
            user.Save();
            return true;
        }

        // Set the user's password to never expire
        public bool SetPasswordNeverExpires(string username)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            user.PasswordNeverExpires = true;
            user.Save();
            return true;
        }

        // Enable a user account
        public bool EnableAdUser(string username)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            user.Enabled = true;
            user.Save();
            return true;
        }

        // Disable a user account
        public bool DisableAdUser(string username)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            user.Enabled = false;
            user.Save();
            return true;
        }

        // Move a user to a new organizational unit
        public bool MoveAdUser(string username, string newLocation)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);

            using (var entry = (DirectoryEntry)user.GetUnderlyingObject())
            {
                entry.MoveTo(new DirectoryEntry($"LDAP://{newLocation}"));
                entry.CommitChanges();
            }

            return true;
        }

        // Unlock a user account
        public bool UnlockAdUser(string username)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            user.UnlockAccount();
            user.Save();
            return true;
        }

        // Remove a user from AD
        public bool RemoveAdUser(string username)
        {
            using var context = CreatePrincipalContext();
            var user = GetUserPrincipal(context, username);
            user.Delete();
            return true;
        }

        // // Get all groups with optional filtering
        // public List<LdapGroup> GetAllAdGroups(string? filter = null)
        // {
        //     using var context = CreatePrincipalContext();
        //     GroupPrincipal group = new(context)
        //     {
        //         Name = string.IsNullOrEmpty(filter) ? null : $"{filter}*"
        //     };

        //     return SearchPrincipals(group)
        //         .Select(g => new LdapGroup(g))
        //         .ToList();
        // }

        // Add a new group to AD
        public LdapGroup AddAdGroup(LdapGroup group)
        {
            using var context = CreatePrincipalContext();
            var newGroup = new GroupPrincipal(context)
            {
                Name = group.Name,
                Description = group.Description
            };
            newGroup.Save();

            return new LdapGroup(newGroup);
        }

        // Get a group by name
        public LdapGroup GetAdGroup(string groupName)
        {
            using var context = CreatePrincipalContext();
            return new LdapGroup(GetGroupPrincipal(context, groupName));
        }

        // Check if a group exists
        public bool AdGroupExists(string groupName)
        {
            using var context = CreatePrincipalContext();
            return GetGroupPrincipal(context, groupName) != null;
        }

        // Add a user to a group
        public bool AddAdUserToGroup(string username, string groupName)
        {
            using var context = CreatePrincipalContext();
            var group = GetGroupPrincipal(context, groupName);
            var user = GetUserPrincipal(context, username);
            group.Members.Add(user);
            group.Save();
            return true;
        }

        // Remove a user from a group
        public bool RemoveAdUserFromGroup(string username, string groupName)
        {
            using var context = CreatePrincipalContext();
            var group = GetGroupPrincipal(context, groupName);
            var user = GetUserPrincipal(context, username);
            group.Members.Remove(user);
            group.Save();
            return true;
        }

        // Remove a group from AD
        public bool RemoveAdGroup(string groupName)
        {
            using var context = CreatePrincipalContext();
            var group = GetGroupPrincipal(context, groupName);
            group.Delete();
            return true;
        }

        // Helper methods
        private static UserPrincipal GetUserPrincipal(PrincipalContext context, string account)
        {
            return UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, account)
                   ?? throw new InvalidOperationException($"User {account} not found.");
        }

        private static GroupPrincipal GetGroupPrincipal(PrincipalContext context, string groupName)
        {
            return GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName)
                   ?? throw new InvalidOperationException($"Group {groupName} not found.");
        }

        private static IEnumerable<T> SearchPrincipals<T>(T principal, int? start = null, int? end = null) where T : Principal
        {
            var results = new PrincipalSearcher(principal).FindAll(); // This returns PrincipalSearchResult<T>
            if (start.HasValue && end.HasValue)
            {
                return results.Skip(start.Value).Take(end.Value - start.Value).Cast<T>();
            }

            return results.Cast<T>();
        }


            }
}

