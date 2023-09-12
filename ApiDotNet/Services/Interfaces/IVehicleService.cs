using ApiDotNet.Models;

namespace ApiDotNet.Services.Interfaces;

public interface IVehicleService
{
    Task<List<Vehicle>> GetAllAsync();
}