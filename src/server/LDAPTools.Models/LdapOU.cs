using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

namespace LDAPTools.Models;

public class LdapOU
{
    public LdapOU() { }

    public LdapOU(DirectoryEntry ou)
    {
        Guid = ou.Guid;
        DistinguishedName = ou.Properties["distinguishedName"].Value?.ToString() ?? string.Empty;
        Name = ou.Properties["name"].Value?.ToString() ?? string.Empty;
        Description = ou.Properties["description"].Value?.ToString() ?? string.Empty;
    }

    public Guid? Guid { get; set; }
    public string DistinguishedName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string ToString() =>
        $"{Name} - {Guid} - {DistinguishedName}";
}
