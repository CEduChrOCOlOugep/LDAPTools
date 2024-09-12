using System.Runtime.Versioning;
using LDAPTools.Services;
using LDAPTools.Models;
using Microsoft.AspNetCore.Mvc;

[assembly: SupportedOSPlatform("windows")]

namespace LDAPTools.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ADController : ControllerBase
{
    private readonly AdToolsService _svc;

    public ADController(AdToolsService svc)
    {
        _svc = svc;
    }

    // Get all users with optional filters
    [HttpGet("users")]
    public IActionResult GetAdUsers([FromQuery] string? fields = null, [FromQuery] string? query = null, [FromQuery] int? start = null, [FromQuery] int? end = null)
    {
        return Ok(_svc.GetAdUsers(query, start, end));
    }

    // Add a new user
    [HttpPost("users")]
    public IActionResult AddAdUser([FromBody] LdapUser user)
    {
        return Ok(_svc.AddAdUser(user));
    }

    // Get a single user by username
    [HttpGet("users/{username}")]
    public IActionResult GetAdUser([FromRoute] string username)
    {
        return Ok(_svc.GetAdUser(username));
    }

    // Check if a user exists
    [HttpGet("users/{username}/exists")]
    public IActionResult AdUserExists([FromRoute] string username)
    {
        return Ok(_svc.AdUserExists(username));
    }

    // Check if a user is a member of a group
    [HttpGet("users/{username}/memberof/{group}")]
    public IActionResult IsAdUserInGroup([FromRoute] string username, [FromRoute] string group)
    {
        return Ok(_svc.IsAdUserInGroup(username, group));
    }

    // // Authenticate a user
    // [HttpPost("users/{username}/authenticate")]
    // public IActionResult AuthenticateUser([FromRoute] string username, [FromQuery] string password)
    // {
    //     return Ok(_svc.AuthenticateAdUser(username, password));
    // }

    // // Change a user's password
    // [HttpPut("users/{username}/password")]
    // public IActionResult ChangeUserPassword([FromRoute] string username, [FromQuery] string newPassword)
    // {
    //     return Ok(_svc.ChangeAdUserPassword(username, newPassword));
    // }

    // // Set password to never expire
    // [HttpPut("users/{username}/password-never-expires")]
    // public IActionResult SetPasswordNeverExpires([FromRoute] string username)
    // {
    //     return Ok(_svc.SetAdUserPasswordNeverExpires(username));
    // }

    // Enable a user
    [HttpPut("users/{username}/enable")]
    public IActionResult EnableUser([FromRoute] string username)
    {
        return Ok(_svc.EnableAdUser(username));
    }

    // Disable a user
    [HttpPut("users/{username}/disable")]
    public IActionResult DisableUser([FromRoute] string username)
    {
        return Ok(_svc.DisableAdUser(username));
    }

    // Move a user
    [HttpPut("users/{username}/move")]
    public IActionResult MoveUser([FromRoute] string username, [FromQuery] string newLocation)
    {
        return Ok(_svc.MoveAdUser(username, newLocation));
    }

    // Unlock a user
    [HttpPut("users/{username}/unlock")]
    public IActionResult UnlockUser([FromRoute] string username)
    {
        return Ok(_svc.UnlockAdUser(username));
    }

    // Remove a user
    [HttpDelete("users/{username}")]
    public IActionResult RemoveUser([FromRoute] string username)
    {
        return Ok(_svc.RemoveAdUser(username));
    }

    // // Get all groups
    // [HttpGet("groups")]
    // public IActionResult GetAllGroups([FromQuery] string? filter = null)
    // {
    //     return Ok(_svc.GetAllGroups(filter));
    // }

    // Add a group
    [HttpPost("groups")]
    public IActionResult AddGroup([FromBody] LdapGroup group)
    {
        return Ok(_svc.AddAdGroup(group));
    }

    // Get a single group
    [HttpGet("groups/{groupName}")]
    public IActionResult GetGroup([FromRoute] string groupName)
    {
        return Ok(_svc.GetAdGroup(groupName));
    }

    // Check if a group exists
    [HttpGet("groups/{groupName}/exists")]
    public IActionResult GroupExists([FromRoute] string groupName)
    {
        return Ok(_svc.AdGroupExists(groupName));
    }

    // Add user to group
    [HttpPost("groups/{groupName}/users/{username}")]
    public IActionResult AddUserToGroup([FromRoute] string groupName, [FromRoute] string username)
    {
        return Ok(_svc.AddAdUserToGroup(username, groupName));
    }

    // Remove user from group
    [HttpDelete("groups/{groupName}/users/{username}")]
    public IActionResult RemoveUserFromGroup([FromRoute] string groupName, [FromRoute] string username)
    {
        return Ok(_svc.RemoveAdUserFromGroup(username, groupName));
    }

    // Remove a group
    [HttpDelete("groups/{groupName}")]
    public IActionResult RemoveGroup([FromRoute] string groupName)
    {
        return Ok(_svc.RemoveAdGroup(groupName));
    }

    // // Get all OUs
    // [HttpGet("ous")]
    // public IActionResult GetAllOUs([FromQuery] string? filter = null)
    // {
    //     return Ok(_svc.GetAllAdOUs(filter));
    // }

    // // Add an OU    
    // [HttpPost("ous")]
    // public IActionResult AddOU([FromBody] LdapOU ou)
    // {
    //     return Ok(_svc.AddAdOU(ou));
    // }

    // // Get a single OU
    // [HttpGet("ous/{ouName}")]
    // public IActionResult GetAdOU([FromRoute] string ouName)
    // {
    //     return Ok(_svc.GetAdOU(ouName));
    // }

    // // Check if an OU exists
    // [HttpGet("ous/{ouName}/exists")]
    // public IActionResult OUExists([FromRoute] string ouName)
    // {
    //     return Ok(_svc.AdOUExists(ouName));
    // }

    // // Remove an OU
    // [HttpDelete("ous/{ouName}")]
    // public IActionResult RemoveOU([FromRoute] string ouName)
    // {
    //     return Ok(_svc.RemoveAdOU(ouName));
    // }

    // // Get all other objects
    // [HttpGet("other")]
    // public IActionResult GetAllOtherObjects()
    // {
    //     return Ok(_svc.GetAllAdOtherObjects());
    // }

    // // Get all objects
    // [HttpGet("all")]
    // public IActionResult GetAllObjects()
    // {
    //     return Ok(_svc.GetAllAdObjects());
    // }

    // // Search Active Directory
    // [HttpGet("find/{filter}")]
    // public IActionResult SearchAD([FromRoute] string filter)
    // {
    //     return Ok(_svc.SearchAd(filter));
    // }

    // // Get API status
    // [HttpGet("status")]
    // public IActionResult GetStatus()
    // {
    //     return Ok(_svc.GetAdStatus());
    // }
}
