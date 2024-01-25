using System.Security.Claims;
using ApiDotNet.Context;
using ApiDotNet.Models;
using ApiDotNet.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CredentialsController : Controller
{
    private readonly UsersContext _usersContext;

    public CredentialsController(UsersContext usersContext)
    {
        _usersContext = usersContext;
    }
    
    [HttpGet]
    public ActionResult GetCredentials()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok(_usersContext.Credentials
            .Where(x => x.User.UserName == username)
            .Select(user => new
            {
                Description = user.Description,
                Credential1 = user.Credential1,
                Credential2 = user.Credential2,
                Credential3 = user.Credential3
            })
        );
    }
    
    [HttpPost]
    public ActionResult AddCredential(CredentialRequest request)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var user = _usersContext.Users.FirstOrDefault(user => user.UserName == username);
        if (user is null)
            return BadRequest();

        _usersContext.Credentials.Add(new Credentials()
        {
            Description = request.Description,
            Credential1 = request.Credential1,
            Credential2 = request.Credential2,
            Credential3 = request.Credential3,
            User = user
        });
        _usersContext.SaveChanges();
        return Ok();
    }
}