using System.DirectoryServices.AccountManagement;

namespace LdapTools.Models;

public class LdapGroup
{
    public LdapGroup()
    {
    }

    public LdapGroup(GroupPrincipal principal)
    {
        Guid = principal.Guid;
        Description = principal.Description;
        DistinguishedName = principal.DistinguishedName;
        Name = principal.Name;
        SamAccountName = principal.SamAccountName;
        IsSecurityGroup = principal.IsSecurityGroup;
        GroupScope = principal.GroupScope;
    }

    public Guid? Guid { get; set; }
    public string Description { get; set; } = string.Empty;
    public string DistinguishedName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SamAccountName { get; set; } = string.Empty;

    public bool? IsSecurityGroup { get; set; }

    public GroupScope? GroupScope { get; set; }

    public override string ToString() =>
        $"{GroupScope} - {Name} - {Guid}";
}