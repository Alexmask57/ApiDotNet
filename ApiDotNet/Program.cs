using ApiDotNet.Services;
using Microsoft.EntityFrameworkCore;
using ApiDotNet.Context;
using ApiDotNet.Extensions;
using ApiDotNet.Services.Interfaces;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.SetUsersDbContext();
builder.SetCachingDbContext();

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.SetRegistrationRules();
builder.SetCors();

builder.Services.AddEndpointsApiExplorer();

builder.SetSwagger();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

// Ajout du service de récupération de véhicules (utile pour la mise en cache)
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.Decorate<IVehicleService, CachedVehicleService>();

builder.Services.AddScoped<TokenService, TokenService>();

builder.Services.AddMemoryCache();

builder.AddJwtAuthentication();

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

app.UseCors(builder.Configuration.GetValue<string>("CorsPolicyName"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();