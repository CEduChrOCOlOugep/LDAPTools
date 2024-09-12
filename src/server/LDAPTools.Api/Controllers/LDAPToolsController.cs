using System.Runtime.Versioning;
using LDAPTools.Services;
using Microsoft.AspNetCore.Mvc;

[assembly: SupportedOSPlatform("windows")]

namespace LDAPTools.Api.Controllers;

[Route("[controller]")]
public class LdapToolsController(LdapToolsService svc) : ControllerBase
{
    private readonly LdapToolsService _svc = svc;

    [HttpGet("[action]")]
    public IActionResult GetAdUsers([FromQuery] int limit = 10)
    {
        return Ok(_svc.GetAdUsers(limit));
    }

    [HttpGet("[action]/{search}")]
    public IActionResult FindAdUsers([FromRoute] string search, [FromQuery] int limit = 10)
    {
        return Ok(_svc.FindAdUsers(search, limit));
    }

    [HttpGet("[action]/{account}")]
    public IActionResult FindAdUser([FromRoute] string account)
    {
        return Ok(_svc.FindAdUser(account));
    }

    [HttpGet("[action]/{account}")]
    public IActionResult GetAdUserGroups([FromRoute] string account)
    {
        return Ok(_svc.GetAdUserGroups(account));
    }
}