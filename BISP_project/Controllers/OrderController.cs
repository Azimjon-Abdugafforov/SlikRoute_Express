using BIS_project.Dtos;
using BIS_project.Helper;
using BIS_project.Models;
using BIS_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using OfficeOpenXml.Utils;
using Org.BouncyCastle.Crypto.Digests;

namespace BIS_project.Controllers;
[Route("api/v1/base-order")]
[ApiController]
public class OrderController : ControllerBase
{
    public string comment { get; set; } = string.Empty;

    private readonly OrderService _orderService;
    private readonly FileSaver _fileSaver;
    public OrderController(OrderService service, FileSaver saver)
    {
        _orderService = service;
        _fileSaver = saver;
    }
    [HttpPost("user", Name = "CreateOrder")]
    public async Task<APIResponse> CreateOrder(BaseOrderDto order)
    {
        try
        {
            return await _orderService.CreateBaseOrder(order);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpPut("user/update/{id}", Name = "GenerateOrder")]
    public async Task<APIResponse> GenerateOrder(int id, [FromForm] UpdateOrderDto dto)
    {
        try
        {
            List<Image> images = await _fileSaver.UploadFiles(dto.images, "orders/images");
            var result = await _orderService.UpdateOrder(dto.Comment, images, id);
            if (result == null)
            {
                return new APIResponse(400, null, "Order nor found");
            }
            var res = Ok(result);
            return new APIResponse(200, res, "");
        }
        catch (Exception e)
        {
            return new APIResponse(400, null, e.Message);
        }
    }

    [HttpPut("admin/append", Name = "AppendEmployees")]
    public async Task<bool> AppendEmployees(OrderByAdminDto dto, int id)
    {
        try
        {
            return await _orderService.AdminUpdateShip(dto, id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("by-user",Name = "GetByClient")]
    public async Task<IActionResult> GetOrderByClient(string username)
    {
        try
        {
            var order = await _orderService.GetOrderByClient(username);
            return Ok(order);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("set-cost/{id}", Name = "SetOrderCost")]
    public async Task<APIResponse> SetOrderCost(int id, int cost, float distance )
    {
        return new APIResponse(200, result: await _orderService.setOrderCost(id, cost, distance), "");
    }

    [HttpPut("upload-start-photos", Name = "UploadPhotos")]
    public async Task<IActionResult> UploadPhotos(int id, IFormFileCollection files)
    {
        try
        {
            List<Image> images = await _fileSaver.UploadFiles(files, "orders/images");
            var result = await _orderService.UploadStartImages(id, images);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("some error occured!");
        }
    }
        [HttpPut("start-ship", Name = "StartShipDriver")]
        public async Task<IActionResult> StartShipDriver(int id)
    {
        try
        {
            return Ok(await _orderService.StartShipping(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("error occured");
        }
    }
    [HttpPut("finish-ship", Name = "FinishShip")]
    public async Task<IActionResult> FinishShip(int id)
    {
        return Ok(await _orderService.FinishShipping(id));
    }

    [HttpPut("rate-order", Name = "RatingOrder")]
    public async Task<IActionResult> RatingOrder(int id, int rating)
    {
        try
        {
            return Ok(await _orderService.RateOrder(id, rating));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Something went wring");
        }
    }
    [HttpPut("upload-finish-photos", Name = "UploadFinishPhotos")]
    public async Task<IActionResult> UploadFinishPhotos(int id, IFormFile photos)
    {
        try
        {
            return Ok(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("driver-emergency-report", Name = "DriverEmergency")]
    public async Task<IActionResult> DriverEmergency(int id, string smth)
    {
        return Ok(smth);
    }
    

    [HttpGet("get-all", Name = "GetAllOrders")]
    public async Task<List<Order?>> GetAllOrders()
    {
        try
        {
            return await _orderService.GetAllOrders();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    
}