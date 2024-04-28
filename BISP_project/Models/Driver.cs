using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIS_project.Models;

public class Driver
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Image DriverPhoto { get; set; }
    public Image LicensePhoto { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string DriverFullName { get; set; }
    public string PhoneNumber { get; set; }
    public  Truck Truck { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; } = string.Empty;
    public Branch Branch { get; set; }
    public string? Email { get; set; }
}