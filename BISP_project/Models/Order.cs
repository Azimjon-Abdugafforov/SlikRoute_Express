using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIS_project.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public List<Employee>? Employees { get; set; }// admin sets
    public float? Distance { get; set; } //generates automatically, distance between start and end points
    public Driver? Driver { get; set; } // updates by admin
    public Client? Client { get; set; }// client sets
    public string? Status { get; set; }// pending, active, shipped
    [Column(TypeName = "timestamptz")] // Specify PostgreSQL type
    public DateTime? StartTime { get; set; } // driver sets
    [Column(TypeName = "timestamptz")] // Specify PostgreSQL type
    public DateTime? FinishTime { get; set; }// driver sets
    [MaxLength(500)]
    public string? Comment { get; set; }//client sets
    public List<string>? StartPoint { get; set; } // clientsets,  map startpoint
    public List<string>? EndPoint { get; set; }//client sets,  map endpoint
    public int  TotalAmount { get; set; } //generates automatically, means the amount of money of shipping
    public List<Image>? ProductImages { get; set; }// 2nd step client sets, images
    public List<Image>? StartImage { get; set; }// driver sets
    public List<Image>? EndImage { get; set; }// driver sets,
    public int? Rating { get; set; } // the client can rate it at the end of the ship with starts
    public Region? FromRegion { get; set; }// client
    public District? FromDistrict { get; set; }// client
    public Region? ToRegion { get; set; }//client
    public District? ToDistrict { get; set; }//client
    public string? LoadDayTime { get; set; }//client
    public DateOnly FromLoadTime { get; set; }//client
    public DateOnly ToLoadTime { get; set; }//client
    [Column(TypeName = "timestamptz")] // Specify PostgreSQL type
    public DateTime CreatedAt { get; set; }//auto generates
    public string? PaymentType { get; set; }// client, uzum pay, payme, humo,
    public List<string>? Services { get; set; } // client, packing, unpacking, car shipping, storage, debris removal,
    public string? HomeType { get; set; } // client, apartment, house
    
}
