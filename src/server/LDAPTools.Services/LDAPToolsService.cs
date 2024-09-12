using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using LDAPTools.Models;
using Microsoft.Extensions.Configuration;

[assembly:SupportedOSPlatform("windows")]
namespace LDAPTools.Services;

public class LdapToolsService
{
    private readonly string _domain;
    //private readonly string _serviceAccountUsername;
    //private readonly string _serviceAccountPassword;

    public LdapToolsService(IConfiguration configuration)
    {
        _domain = configuration["ActiveDirectory:Domain"] ?? throw new InvalidOperationException("Active Directory domain must be specified in the configuration.");
        //_serviceAccountUsername = configuration["ActiveDirectory:ServiceAccountUsername"];
        //_serviceAccountPassword = configuration["ActiveDirectory:ServiceAccountPassword"];
    }

    private PrincipalContext CreatePrincipalContext()
    {
        return new PrincipalContext(ContextType.Domain, _domain
        //, _serviceAccountUsername, _serviceAccountPassword
        );
    }

    public List<LdapUser> GetAdUsers(string? query = null, int? start = null, int? end = null)
    {
        using var context = CreatePrincipalContext();
        UserPrincipal principal = new(context)
        {
            Enabled = true
        };

        return SearchPrincipals(principal)
            .Select(x => new LdapUser(x))
            .ToList();
    }

    public List<LdapUser> FindAdUsers(string search, int? start = null, int? end = null)
    {
        using var context = CreatePrincipalContext();
        UserPrincipal principal = new(context)
        {
            Enabled = true,
            SamAccountName = $"{search}*"
        };

        return SearchPrincipals(principal)
            .Select(x => new LdapUser(x))
            .ToList();
    }

    public LdapUser FindAdUser(string account)
    {
        using var context = CreatePrincipalContext();
        return new(GetUserPrincipal(context, account));
    }

    public List<LdapGroup> GetAdUserGroups(string account)
    {
        using var context = CreatePrincipalContext();
        return GetUserPrincipal(context, account)
            .GetGroups()
            .Cast<GroupPrincipal>()
            .Select(x => new LdapGroup(x))
            .ToList();
    }

    static UserPrincipal GetUserPrincipal(PrincipalContext context, string account) =>
        UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, account);

    static IEnumerable<T> SearchPrincipals<T>(T principal, int? start = null, int? end = null)
    where T : Principal =>
        new PrincipalSearcher(principal)
            .FindAll()      
            .Skip(start ?? 0)
            .Take(end ?? int.MaxValue)
            .Cast<T>();
}
