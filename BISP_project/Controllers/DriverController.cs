using BIS_project.Dtos;
using BIS_project.Helper;
using BIS_project.Models;
using BIS_project.Request;
using BIS_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIS_project.Controllers;
[Route("api/v1/driver")]
[ApiController]
public class DriverController : ControllerBase
{
    private readonly DriverService _driverService;
    private readonly FileSaver _saver;
    private readonly TruckService _truckService;

    public DriverController(DriverService service, FileSaver save, TruckService truckService)
    {
        _driverService = service;
        _saver = save;
        _truckService = truckService;
    }

    [HttpGet(template:"get-driver-order")]
    public async Task<APIResponse> GetDriverOrders(string name)
    {
        try
        {
            List<Order> driverOrders = await _driverService.GetDriverQuotes(name);
            return new APIResponse(200, driverOrders, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new APIResponse(400, "", e.Message);
        }
    }

    [HttpPost(Name = "createDriver")]
    public async Task<APIResponse> CreateDriver([FromForm] DriverModel model)
    {
        try
        {
            var truckImages = await _saver.SaveImageListAsync(model.TruckImages, "Trucks/images");
            var truck = new TruckDto()
            {
                ManufacturedYear = model.Driver.ManufacturedYear,
                TruckModel = model.Driver.TruckModel,
                TruckNumber = model.Driver.TruckNumber,
                TruckStatus = model.Driver.TruckStatus
            };
            var truckCreated = await _truckService.CreateTruck(truck, truckImages);
            if (truckCreated != null && truckCreated.Id != 0)   
            {
                var img = await _saver.FileSaveAsync(model.Photo, "Drivers/images");
                var license = await _saver.FileSaveAsync(model.License, "Drivers/images");
                var result = await _driverService.CreateDriver(model.Driver, img, license, truckCreated.Id);
                return new APIResponse(200, result, ""); 
            }
            return new APIResponse(500, "", "error occured while saving the driver");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new APIResponse(500, "", e.Message);
        }
    }
    [HttpGet(Name = "getDrivers")]
    public async Task<APIResponse> GetDrivers()
    {
        return await _driverService.GetDrivers();
    }
    
}