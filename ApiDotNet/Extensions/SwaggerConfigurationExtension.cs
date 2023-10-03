using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiDotNet.Extensions;

/// <summary>
/// Extension permettant de configurer Swagger
/// </summary>
public static class SwaggerConfigurationExtension
{
    /// <summary>
    /// Met en place le swagger de l'application
    /// </summary>
    /// <param name="builder"></param>
    public static void SetSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SetDocumentationInfos();
            options.AddAuthentification();
            options.AllowXmlComment();
        });
    }

    /// <summary>
    /// Met à jour la documentation du swagger de l'application
    /// </summary>
    /// <param name="options"></param>
    private static void SetDocumentationInfos(this SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ApiDotNet",
            Description = "An ASP.NET Core Web API for managing ToDo items",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "Example License",
                Url = new Uri("https://example.com/license")
            }
        });
    }

    /// <summary>
    /// Ajoute les fonctionnalités d'authentification sur l'interface de swagger
    /// </summary>
    /// <param name="options"></param>
    private static void AddAuthentification(this SwaggerGenOptions options)
    {
        // Ajoute une fonctionnalité d'authentification
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        // Sur Ajoute les cadenas sur les controlleurs
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    }

    /// <summary>
    /// Ajoute les commentaires XML de l'application à la documentation Swagger de l'application
    /// </summary>
    /// <param name="options"></param>
    private static void AllowXmlComment(this SwaggerGenOptions options)
    {
        // Permet à Swagger de récuperer les commentaires XML pour documenter 
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
}