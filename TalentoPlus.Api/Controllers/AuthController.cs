using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmployeeService _employeeService;
    private readonly IConfiguration _configuration;
    
    public AuthController(
        UserManager<ApplicationUser> userManager,
        IEmployeeService employeeService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _employeeService = employeeService;
        _configuration = configuration;
    }
    
    /// <summary>
    /// Admin login - Returns JWT token for admin users
    /// </summary>
    [HttpPost("admin/login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        
        if (user == null || !user.IsAdmin)
        {
            return Unauthorized(new { message = "Invalid credentials or user is not an admin." });
        }
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        
        if (!isPasswordValid)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }
        
        var token = GenerateJwtToken(user.Id, user.Email!, "Admin");
        
        return Ok(new
        {
            token,
            email = user.Email,
            fullName = user.FullName,
            role = "Admin"
        });
    }
    
    /// <summary>
    /// Employee login - Returns JWT token for employees
    /// </summary>
    [HttpPost("employee/login")]
    public async Task<IActionResult> EmployeeLogin([FromBody] LoginDto dto)
    {
        var employee = await _employeeService.ValidateLoginAsync(dto.Document, dto.Email);
        
        if (employee == null)
        {
            return Unauthorized(new { message = "Invalid credentials. Document and email do not match." });
        }
        
        var token = GenerateJwtToken(employee.Id.ToString(), employee.Email, "Employee", employee.Document);
        
        return Ok(new
        {
            token,
            employeeId = employee.Id,
            email = employee.Email,
            fullName = employee.FullName,
            role = "Employee"
        });
    }
    
    private string GenerateJwtToken(string userId, string email, string role, string? document = null)
    {
        var key = _configuration["Jwt:Key"] ?? "a-string-secret-at-least-256-bits-long";
        var issuer = _configuration["Jwt:Issuer"] ?? "TalentoPlus";
        var audience = _configuration["Jwt:Audience"] ?? "TalentoPlus";
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };
        
        if (!string.IsNullOrEmpty(document))
        {
            claims.Add(new Claim("Document", document));
        }
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}