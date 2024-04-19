using BIS_project.AppData;
using BIS_project.Dtos;
using BIS_project.IServices;
using BIS_project.Models;
using Microsoft.EntityFrameworkCore;

namespace BIS_project.Services;

public class TruckService: ITruckService
{
    private readonly DataContext _dataContext;

    public TruckService(DataContext data)
    {
        _dataContext = data;
    }
    
    public async Task<Truck> CreateTruck(TruckDto truck, List<Image> img)
    {
        try
        {
            var t = new Truck()
            {
                TruckImages = img,
                ManufacturedYear = truck.ManufacturedYear,
                TruckModel = truck.TruckModel,
                TruckNumber = truck.TruckNumber,
                TruckStatus = truck.TruckStatus
            };

            await _dataContext.Trucks.AddAsync(t);
            await _dataContext.SaveChangesAsync();
            return t;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public Task<bool> DeleteTruck(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> UpdateTruck(TruckDto truck)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Truck>> GetAllTruck()
    {
        return await _dataContext.Trucks
            .Include(e=>e.TruckImages)
            .ToListAsync(); 
    }
}