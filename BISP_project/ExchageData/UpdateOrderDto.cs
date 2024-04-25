using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace BIS_project.Dtos;

public class UpdateOrderDto
{
    
    [FromForm(Name = "comment")]
    public string Comment { get; set; }
    
    [FromForm(Name = "images")]
    public IFormFileCollection images { get; set; }
}