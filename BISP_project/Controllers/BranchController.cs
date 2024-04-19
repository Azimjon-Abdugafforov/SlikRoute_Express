
using BIS_project.Dtos;
using BIS_project.IServices;
using BIS_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace BIS_project.Controllers;
[Route("api/v1/branch")]
[ApiController]
public class BranchController : GenericController<Branch, BranchDto> 
{
    public BranchController(IGenericService<Branch, BranchDto> br) : base(br)
    {
        
    }
}