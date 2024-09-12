using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using LdapTools.Api.Models;

[assembly: SupportedOSPlatform("windows")]

namespace LdapTools.Api.Services;

public class LdapToolsService
{
    private readonly string _domain;
    //private readonly string _serviceAccountUsername;
    //private readonly string _serviceAccountPassword;

    public LdapToolsService(IConfiguration configuration)
    {
        _domain = configuration["ActiveDirectory:Domain"] ??
                  throw new InvalidOperationException(
                      "Active Directory domain must be specified in the configuration.");
        //_serviceAccountUsername = configuration["ActiveDirectory:ServiceAccountUsername"];
        //_serviceAccountPassword = configuration["ActiveDirectory:ServiceAccountPassword"];
    }

    public List<LdapUser> GetAdUsers(int limit = 10)
    {
        PrincipalContext context = new(ContextType.Domain);
        UserPrincipal principal = new(context)
        {
            Enabled = true
        };

        return SearchPrincipals(principal, limit)
            .Select(x => new LdapUser(x))
            .ToList();
    }

    public List<LdapUser> GetAllAdUsers()
    {
        PrincipalContext context = new PrincipalContext(ContextType.Domain);
        UserPrincipal principal = new UserPrincipal(context) { Enabled = true };

        PrincipalSearcher searcher = new PrincipalSearcher(principal);
        var results = searcher.FindAll();

        return results.Cast<UserPrincipal>()
                      .Select(x => new LdapUser(x))
                      .ToList();
    }

    public List<LdapUser> FindAdUsers(string search, int limit = 10)
    {
        PrincipalContext context = new(ContextType.Domain);

        UserPrincipal principal = new(context)
        {
            Enabled = true,
            SamAccountName = $"{search}*"
        };

        return SearchPrincipals(principal, limit)
            .Select(x => new LdapUser(x))
            .ToList();
    }

    public LdapUser FindAdUser(string account)
    {
        return new LdapUser(GetUserPrincipal(account));
    }

    public List<LdapGroup> GetAdUserGroups(string account)
    {
        return GetUserPrincipal(account)
            .GetGroups()
            .Cast<GroupPrincipal>()
            .Select(x => new LdapGroup(x))
            .ToList();
    }

    private static UserPrincipal GetUserPrincipal(string account)
    {
        return UserPrincipal.FindByIdentity(
            new PrincipalContext(ContextType.Domain),
            IdentityType.SamAccountName,
            account
        );
    }

    private static IEnumerable<T> SearchPrincipals<T>(T principal, int limit = 10)
        where T : Principal
    {
        return new PrincipalSearcher(principal)
            .FindAll()
            .Take(limit)
            .Cast<T>();
    }


}