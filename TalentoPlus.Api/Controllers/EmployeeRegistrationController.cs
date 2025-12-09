using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;

namespace TalentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeRegistrationController : ControllerBase
{
    private readonly IEmployeeRegistrationService _registrationService;
    
    public EmployeeRegistrationController(IEmployeeRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    
    /// <summary>
    /// Self-registration for new employees - Public endpoint
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] EmployeeRegistrationDto dto)
    {
        try
        {
            var employee = await _registrationService.RegisterAsync(dto);
            return CreatedAtAction(nameof(Register), new { id = employee.Id }, employee);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}