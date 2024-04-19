using BIS_project.Helper;
using BIS_project.Models;

namespace BIS_project.Dtos;

public class BaseOrderDto
{
    public int FromRegion { get; set; }
    public int FromDistrict { get; set; }
    public int ToRegion { get; set; }
    public int ToDistrict { get; set; }
    public string LoadDayTime { get; set; }
    public DateOnly FromLoadTime { get; set; }// flexible loading time
    public DateOnly ToLoadTime { get; set; }
    public List<string> StartPoint { get; set; }
    public List<string> EndPoint { get; set; } 
    public string PaymentType { get; set; } //card, cash ...
    public List<string> Services { get; set; } // packaging...
    public string HomeType { get; set; } // apartment, house...
    public string Email { get; set; }
    public string PhoneNumber { get; set;}
    public string FullName { get; set; }
} 