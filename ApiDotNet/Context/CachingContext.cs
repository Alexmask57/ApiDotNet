using Bogus;
using Microsoft.EntityFrameworkCore;
using ApiDotNet.Models;

namespace ApiDotNet.Context;

public class CachingContext : DbContext
{
    public CachingContext (DbContextOptions<CachingContext> options) : base(options) { }
    public DbSet<Vehicle>? Vehicles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed database with some dummy vehicles
        var id = 1;
        var vehicles = new Faker<Vehicle>()
            .RuleFor(v => v.Id, f => id++)
            .RuleFor(v => v.Manufacturer, f => f.Vehicle.Manufacturer())
            .RuleFor(v => v.Model, f => f.Vehicle.Model())
            .RuleFor(v => v.Type, f => f.Vehicle.Type())
            .RuleFor(v => v.Vin, f => f.Vehicle.Vin())
            .RuleFor(v => v.Fuel, f => f.Vehicle.Fuel());

        // generate 500 vehicles
        modelBuilder
            .Entity<Vehicle>()
            .HasData(vehicles.GenerateBetween(500, 500));
    }

}