using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApiDotNet.Context;
using ApiDotNet.Models.Authentication;
using ApiDotNet.Models.Dto;
using ApiDotNet.Services;

namespace ApiDotNet.Controllers;

/// <summary>
/// Controlleur d'authentification
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UsersContext _usersContext;
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<ApplicationUser> userManager, UsersContext usersContext,
        TokenService tokenService, RoleManager<ApplicationRole> roleManager, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _usersContext = usersContext;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        // Todo SaveChangeAsync
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var defaultRole = _roleManager.Roles.FirstOrDefault(x => x.Name == "Default");
        if (defaultRole == null)
        {
            _logger.LogCritical("Le rôle \"défault\" n'existe pas dans la base de données");
            return StatusCode(500);
        }

        var newUser = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email
        };
        var result = await _userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return BadRequest(ModelState);
        }

        var resultRole = await _userManager.AddToRoleAsync(newUser, "Default");
        if (!resultRole.Succeeded)
        {
            foreach (var error in resultRole.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return BadRequest(ModelState);
        }

        RegistrationResponse response = new RegistrationResponse()
        {
            Email = request.Email,
            Username = request.Username,
            Role = "Default"
        };
        return CreatedAtAction(nameof(Register), new { email = request.Email }, response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
            return BadRequest("Bad credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
            return BadRequest("Bad credentials");

        var userRole = await _userManager.GetRolesAsync(managedUser);

        var userInDb = _usersContext.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userInDb is null)
            return Unauthorized();
        var accessToken = userRole.Count != 0 ? _tokenService.CreateToken(userInDb, userRole) : _tokenService.CreateToken(userInDb);
        
        _usersContext.UserTokens.Add(new IdentityUserToken<string>()
        {
            UserId = userInDb.Id, LoginProvider = "AuthController", Name = "Authentication token", Value = accessToken
        });
        await _usersContext.SaveChangesAsync();
        
        return Ok(new AuthResponse
        {
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        });
    }

    [HttpPut]
    [Route("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
            return BadRequest("Bad credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
            return BadRequest("Bad credentials");
        var result = await _userManager.ChangePasswordAsync(managedUser, request.Password, request.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return BadRequest(ModelState);
        }
        request.Password = String.Empty;
        request.NewPassword = String.Empty;
        request.NewPasswordConfirmed = String.Empty;
        return AcceptedAtAction(nameof(ChangePassword), new { email = request.Email }, request);
    }

    [HttpGet]
    [Route("CheckToken")]
    [Authorize(Roles = "Admin")]
    public ActionResult<bool> CheckToken(string token)
    {
        return Ok(_tokenService.CheckTokenIsValidWithSecret(token));
    }

    [HttpGet]
    [Route("GetAllUsers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetAllUsers()
    {
        return Ok(_usersContext.Users.Select(x =>
        new
        {
            UserName = x.UserName,
            Email = x.Email,
            Roles = new List<string>(x.UserRoles.Select(role => role.Role.Name)!)
        }));
    }
}