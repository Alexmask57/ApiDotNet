using ApiDotNet.Config;
using ApiDotNet.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiDotNet.Extensions;

/// <summary>
/// Extension permettant de configurer les connexions aux bases de données
/// </summary>
public static class DbConfigurationExtension
{
    
    /// <summary>
    /// Configure la connexion à la base de données liée à l'authentification
    /// </summary>
    /// <param name="builder"></param>
    public static void SetUsersDbContext(this WebApplicationBuilder builder)
    {
        var host = builder.Configuration["DBHOST"] ?? "localhost";
        var port = builder.Configuration["DBPORT"] ?? "3306";
        var password = builder.Configuration["MYSQL_PASSWORD"] ?? builder.Configuration.GetConnectionString("MYSQL_PASSWORD");
        var userid = builder.Configuration["MYSQL_USER"] ?? builder.Configuration.GetConnectionString("MYSQL_USER");
        var usersDataBase = builder.Configuration["MYSQL_DATABASE"] ?? builder.Configuration.GetConnectionString("MYSQL_DATABASE");
        
        var connectionString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
        
        // var connectionString = builder.Configuration.GetConnectionString("JWTAUTHENTICATION");
        Console.WriteLine("connectionString");
        Console.WriteLine(connectionString);
        builder.Services.AddDbContext<UsersContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                // The following three options help with debugging, but should
                // be changed or removed for production.
                // .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
    }

    /// <summary>
    /// Configure la connexion à la base de données de cache
    /// </summary>
    /// <param name="builder"></param>
    public static void SetCachingDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("MySqlCachingConnectionString");

        builder.Services.AddDbContext<CachingContext>(dbContextOptions =>
            dbContextOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }

    private static bool IsDeveloppement()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}