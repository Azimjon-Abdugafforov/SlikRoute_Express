using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BIS_project.AppData;
using BIS_project.Dtos;
using BIS_project.Helper;
using BIS_project.IServices;
using BIS_project.Models;
using Microsoft.EntityFrameworkCore;

namespace BIS_project.Services;

public class DriverService : IDriverService
{
    private readonly DataContext _dataContext;
    private readonly AuthService _authService;


    public DriverService(DataContext data, AuthService auth)
    {
        _dataContext = data;
        _authService = auth;
    }

    public async Task<List<Order>> GetDriverQuotes(string name)
    {
        try
        {
            var orders = await _dataContext.Orders
                .Where(e => e.Driver.DriverFullName == name)
                .Include(e=> e.Driver.Truck.TruckImages)
                .Include(e=> e.Employees)
                .Include(e=> e.FromDistrict)
                .Include(e=> e.ToDistrict)
                .Include(e=> e.Client)
                .Include(e=> e.ProductImages)
                .Include(e=> e.EndImage)
                
                .ToListAsync();

            return orders;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    
    public async Task<bool> CreateDriver(DriverDto driver, Image img, Image license, int truckId)
    {
        try
        {
            var d = new Driver()
            {
                Truck = await _dataContext.Trucks.FindAsync(truckId),
                Status = "Available to ship",
                DriverFullName = driver.DriverFullName,
                IsActive = true,
                DateOfBirth = driver.DateOfBirth,
                DriverPhoto = img,
                LicensePhoto = license,
                PhoneNumber = driver.PhoneNumber, 
                Branch = await _dataContext.Branches.FindAsync(driver.BranchId)
            };
            await _dataContext.Drivers.AddAsync(d);
            await _dataContext.SaveChangesAsync();
            var user = new UserRegisterDto()
            {
                Firstname = driver.DriverFullName,
                Password = GenerateRandomPassword(),
                Username = driver.Email,
            };
            await _authService.DriverRegister(user);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    private string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        int passwordLength = 6;
        string password = new string(Enumerable.Repeat(chars, passwordLength)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return password;
    }
    

    public async Task<APIResponse> GetDrivers()
    {
        try
        {
            var empList = await _dataContext.Drivers
                .Include(e=> e.LicensePhoto)
                .Include(e=> e.DriverPhoto)
                .Include(e=> e.Truck.TruckImages)
                .ToListAsync();
            return new APIResponse(200, empList, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<APIResponse> DeleteDriver(int id)
    {
        throw new NotImplementedException();
    }
}