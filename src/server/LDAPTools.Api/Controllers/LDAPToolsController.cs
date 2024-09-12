using System.Runtime.Versioning;
using LdapTools.Api.Services;
using Microsoft.AspNetCore.Mvc;

[assembly: SupportedOSPlatform("windows")]

namespace LdapTools.Api.Controllers;

[Route("[controller]")]
public class LdapToolsController(LdapToolsService svc) : ControllerBase
{
    private readonly LdapToolsService svc = svc;

    [HttpGet("[action]")]
    public IActionResult GetAdUsers([FromQuery] int limit = 10)
    {
        return Ok(svc.GetAdUsers(limit));
    }

    [HttpGet("[action]/{search}")]
    public IActionResult FindAdUsers([FromRoute] string search, [FromQuery] int limit = 10)
    {
        return Ok(svc.FindAdUsers(search, limit));
    }

    [HttpGet("[action]/{account}")]
    public IActionResult FindAdUser([FromRoute] string account)
    {
        return Ok(svc.FindAdUser(account));
    }

    [HttpGet("[action]/{account}")]
    public IActionResult GetAdUserGroups([FromRoute] string account)
    {
        return Ok(svc.GetAdUserGroups(account));
    }
}