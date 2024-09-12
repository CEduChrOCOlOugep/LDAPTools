using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using LDAPTools.Models;
using Microsoft.Extensions.Configuration;  

[assembly:SupportedOSPlatform("windows")]
namespace LDAPTools.Services;

public class LdapToolsService
{
    private readonly string _domain;

    public LdapToolsService(IConfiguration configuration)
    {
        _domain = configuration["ActiveDirectory:Domain"] ?? throw new InvalidOperationException("Active Directory domain must be specified in the configuration.");
    }

    public List<LdapUser> GetAdUsers(int limit = 10)
    {
        PrincipalContext context = new(ContextType.Domain, _domain);
        UserPrincipal principal = new(context)
        {
            Enabled = true
        };

        return SearchPrincipals(principal, limit)
            .Select(x => new LdapUser(x))
            .ToList();
    }

    public List<LdapUser> FindAdUsers(string search, int limit = 10)
    {
        PrincipalContext context = new(ContextType.Domain, _domain);

        UserPrincipal principal = new(context)
        {
            Enabled = true,
            SamAccountName = $"{search}*"
        };

        return SearchPrincipals(principal, limit)
            .Select(x => new LdapUser(x))
            .ToList();
    }

    public LdapUser FindAdUser(string account) =>
        new(GetUserPrincipal(account));

    public List<LdapGroup> GetAdUserGroups(string account) =>
        GetUserPrincipal(account)
            .GetGroups()
            .Cast<GroupPrincipal>()
            .Select(x => new LdapGroup(x))
            .ToList();

    static UserPrincipal GetUserPrincipal(string account) =>
        UserPrincipal.FindByIdentity(
            new PrincipalContext(ContextType.Domain),
            IdentityType.SamAccountName,
            account
        );

    static IEnumerable<T> SearchPrincipals<T>(T principal, int limit = 10)
    where T : Principal =>
        new PrincipalSearcher(principal)
            .FindAll()
            .Take(limit)
            .Cast<T>();
}
