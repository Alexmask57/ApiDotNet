using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ApiDotNet.Enum;
using ApiDotNet.Models.Authentication;

namespace ApiDotNet.Services;

public class TokenService
{
    private const int ExpirationMinutes = 30;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public string CreateToken(ApplicationUser user, IList<string> roles, out DateTime tokenExpirationDate)
    {
        tokenExpirationDate = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var claims = CreateClaims(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        // claims.Add(new Claim(ClaimTypes.Role, GetHigherRole(roles)));
        var token = CreateJwtToken(
            claims,
            CreateSigningCredentials(),
            tokenExpirationDate
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Vérifie si le token donné en entrée est valide (en vérifiant à l'aide du secret)
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns></returns>
    public bool CheckTokenIsValidWithSecret(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = false, // Because there is no expiration in the generated token
            ValidateAudience = false, // Because there is no audiance in the generated token
            ValidateIssuer = false, // Because there is no issuer in the generated token
            ValidIssuer = "Sample",
            ValidAudience = "Sample",
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration.GetValue<string>("JwtSecret"))) // The same key as the one that generate the token
        };

        SecurityToken validatedToken;
        try
        {
            IPrincipal principal =
                principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
        catch (SecurityTokenSignatureKeyNotFoundException e)
        {
            return false;
        }

        return true;
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            "apiWithAuthBackend",
            "apiWithAuthBackend",
            claims,
            expires: expiration,
            signingCredentials: credentials
        ) { };

    private List<Claim> CreateClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };
        return claims;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSecret"))
            ){KeyId = "MyKey"},
            SecurityAlgorithms.HmacSha256
        );
    }

    /// <summary>
    /// Récupère le rôle ayant le plus de privilèges
    /// </summary>
    /// <param name="roles">Liste des rôles</param>
    /// <returns>Le rôle ayant le plus de privilèges</returns>
    private string GetHigherRole(IList<string> roles)
    {
        try
        {
            List<RoleEnum> rolesEnum = new List<RoleEnum>();
            foreach (var role in roles)
            {
                rolesEnum.Add(System.Enum.Parse<RoleEnum>(role));
            }
            return rolesEnum.Max().ToString();
        }
        catch (Exception e)
        {
            _logger.LogCritical("Impossible de trouver un des rôles dans l'énumération RoleEnum", e);
            return String.Empty;
        }
    }
}