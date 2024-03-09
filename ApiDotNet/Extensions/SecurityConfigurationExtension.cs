using System.Text;
using ApiDotNet.Context;
using ApiDotNet.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ApiDotNet.Extensions;

/// <summary>
/// Extension permettant de gérer la sécurité de l'application
/// </summary>
public static class SecurityConfigurationExtension
{
    /// <summary>
    /// Mets à jour les CORS de l'application
    /// </summary>
    /// <param name="builder"></param>
    public static void SetCors(this WebApplicationBuilder builder)
    {
        var authorizedUrls = builder.Configuration.GetSection("CORSOrigin").Get<List<string>>();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: builder.Configuration.GetValue<string>("CorsPolicyName") ?? "ApiDotNetCorsPolicy",
                policy  =>
                {
                    policy
                        .WithOrigins(authorizedUrls.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    /// <summary>
    /// Configure les règles d'inscription 
    /// </summary>
    /// <param name="builder"></param>
    public static void SetRegistrationRules(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<UsersContext>();
    }

    /// <summary>
    /// Ajoute l'authentification JWT à l'application
    /// </summary>
    /// <param name="builder"></param>
    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSecret = builder.Configuration.GetValue<string>("JwtSecret") ?? throw new InvalidOperationException();
        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "apiWithAuthBackend",
                    ValidAudience = "apiWithAuthBackend",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret)
                    ){KeyId = "MyKey"},
                };
            });
    }
}