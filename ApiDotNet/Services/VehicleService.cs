using Microsoft.EntityFrameworkCore;
using ApiDotNet.Context;
using ApiDotNet.Models;
using ApiDotNet.Services.Interfaces;

namespace ApiDotNet.Services;

public class VehicleService : IVehicleService
{
    private readonly CachingContext _dbContext;

    public VehicleService(CachingContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        var vehicles = await _dbContext.Vehicles.ToListAsync();

        return vehicles;
    }
}