using ApiDotNet.Config;
using ApiDotNet.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ApiDotNet.Context;
using ApiDotNet.Models.Authentication;
using ApiDotNet.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var mySqlJwtAuthenticationConnectionString = builder.Configuration.GetConnectionString("MySqlJWTAuthenticationConnectionString");
var mySqlCachingConnectionString = builder.Configuration.GetConnectionString("MySqlCachingConnectionString");
var jwtSecret = builder.Configuration.GetValue<string>("JwtSecret") ?? throw new InvalidOperationException();

// Ajout de la connexion à la base de de données "JWTAuthentication" de MySql
builder.Services.AddDbContext<UsersContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(mySqlJwtAuthenticationConnectionString, ServerVersion.AutoDetect(mySqlJwtAuthenticationConnectionString))
        // The following three options help with debugging, but should
        // be changed or removed for production.
        // .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Ajout de la connexion à la base de de données "Caching" de MySql
builder.Services.AddDbContext<CachingContext>(dbContextOptions =>
    dbContextOptions.UseMySql(mySqlCachingConnectionString, ServerVersion.AutoDetect(mySqlCachingConnectionString)));

// Spécifie les règles pour s'inscrire
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

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// TODO Regarder cette histoire de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
        policy  =>
        {
            policy.AllowAnyHeader();
            // policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
            policy.WithOrigins("http://localhost:4200");
            policy.WithOrigins("https://alexmaskapi.azurewebsites.net");
        });
});

builder.Services.AddEndpointsApiExplorer();

// Ajout de Swagger
builder.Services.AddSwaggerGen(options =>
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    
    // Permet à Swagger de récuperer les commentaires XML pour documenter 
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
});

// Ajout de la base de données MongoDB
builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase"));

// Ajout du service de récupération des données concernant les livres
builder.Services.AddSingleton<BooksService>();

// Ajout du service de récupération de véhicules (utile pour la mise en cache)
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.Decorate<IVehicleService, CachedVehicleService>();

// Ajout du service de création de Token JWT
builder.Services.AddScoped<TokenService, TokenService>();

// In-Memory Caching
builder.Services.AddMemoryCache();

// Ajout de l'authentification
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
            ),
        };
    });

var app = builder.Build();

// Migrate database automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UsersContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.UseCors("_myAllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();