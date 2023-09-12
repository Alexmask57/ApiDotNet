using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ApiDotNet.Models;
using ApiDotNet.Services.Interfaces;

namespace ApiDotNet.Controllers;

[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Vehicle> vehicles = await _vehicleService.GetAllAsync();
        return Ok(vehicles);
    }
}