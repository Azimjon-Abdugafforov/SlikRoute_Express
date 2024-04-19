namespace BIS_project.Dtos;
using BIS_project.Models;

public class DriverDto
{
    public string DriverFullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public String Email { get; set; }
    public string PhoneNumber { get; set; }
    public int BranchId { get; set; }
    public string TruckModel { get; set; }
    public string TruckNumber { get; set; }
    public string TruckStatus { get; set; }
    public int ManufacturedYear { get; set; }
}