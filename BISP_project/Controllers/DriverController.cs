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
    public async Task<APIResponse> GetDriverOrders(string email)
    {
        try
        {
            Order driverOrders = await _driverService.GetDriverQuotes(email);
            if(driverOrders.Status == null)
            {
                return new APIResponse(404, "", "No orders found");
            }
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
                Console.WriteLine(img);
                var license = await _saver.FileSaveAsync(model.License, "Licenses/images");
                var result = await _driverService.CreateDriver(model.Driver, img, license, truckCreated.Id);
                return new APIResponse(200, result, "");
            }
            return new APIResponse(500, "", "Error occurred while saving the driver");
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

    [HttpGet("get-driver-archives", Name = "getDriverArchives")]
    public async Task<APIResponse> getDriverArchiveOrders(string userName)
    {
        var orders = await _driverService.driverArchives(userName);
        return new APIResponse(200,  orders, "");
    }
     
}