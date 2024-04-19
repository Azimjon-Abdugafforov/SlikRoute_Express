using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BIS_project.Dtos;
using BIS_project.Helper;
using BIS_project.Models;
using BIS_project.Response;
using BIS_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;

namespace BIS_project.Controllers;
[Route("api/v1/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;
    private readonly EmployeeService _employee;
    
    public AuthController(AuthService service, IConfiguration configuration, EmployeeService emp)
    {
        _employee = emp;
        _authService = service;
        _configuration = configuration;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest( new APIResponse(400, "", "provide the required fields"));
        }
        try
        {
            var user = await _authService.UserLogin(request);
            Employee? employee = await _employee.GetEmployeeByName(user);
            if (user == null)
            {
                return Unauthorized(new APIResponse(401, "", "user not found"));
            }
            if (user.Password != request.Password)
            {
                return Ok(new APIResponse(400, "", "Password is incorrect!"));
            }
                var emp = employee != null? new
                {
                    employee.Firstname,
                    employee.Lastname,
                    employee.Position.Title,
                    employee.DateOfBirth,
                    employee.Image,
                    employee.Branch.BranchName,
                    employee.Branch.District.Name,
                    employee.Branch.Region.RegionName,
                }: null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Role_name)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds); 

            var bearer = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new {user, emp, bearer });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing the login request.");
        }
    }
    [HttpPost("register")]
    public async Task<object> Register(UserRegisterDto request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Username or password is missing.");
        }
        try
        {
            var user = await _authService.UserRegister(request);
            if (user == null)
            {
                return BadRequest("User already exists!");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing the register request.");
        }
    }
  
    [HttpPut("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string username)
    {
        try
        {
            APIResponse result = await _authService.ForgotPassword(username);
            if (result.ResponseCode == 200)
            {
                return Ok(result);
            }
          
            return BadRequest("User not found!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("some error occured!");
        }
    }
    
    
}