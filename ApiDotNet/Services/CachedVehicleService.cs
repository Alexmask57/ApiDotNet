using Microsoft.Extensions.Caching.Memory;
using ApiDotNet.Models;
using ApiDotNet.Services.Interfaces;

namespace ApiDotNet.Services;

public class CachedVehicleService : IVehicleService
{
    private const string VehicleListCacheKey = "VehicleList";
    private readonly IMemoryCache _memoryCache;
    private readonly IVehicleService _vehicleService;

    public CachedVehicleService(
        IVehicleService vehicleService,
        IMemoryCache memoryCache)
    {
        _vehicleService = vehicleService;
        _memoryCache = memoryCache;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(50));

        if (_memoryCache.TryGetValue(VehicleListCacheKey, out List<Vehicle> query))
            return query;

        query = await _vehicleService.GetAllAsync();

        _memoryCache.Set(VehicleListCacheKey, query, cacheOptions);

        return query;
            
    }
}