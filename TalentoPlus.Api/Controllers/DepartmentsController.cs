using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.Interfaces;

namespace TalentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    
    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    
    /// <summary>
    /// Get all departments - Public endpoint
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }
    
    /// <summary>
    /// Get department by ID - Public endpoint
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        
        if (department == null)
        {
            return NotFound(new { message = "Department not found." });
        }
        
        return Ok(department);
    }
}