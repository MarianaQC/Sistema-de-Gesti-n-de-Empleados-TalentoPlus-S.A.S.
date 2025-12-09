using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;

namespace TalentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IPdfService _pdfService;
    private readonly IExcelImportService _excelImportService;
    
    public EmployeesController(
        IEmployeeService employeeService,
        IPdfService pdfService,
        IExcelImportService excelImportService)
    {
        _employeeService = employeeService;
        _pdfService = pdfService;
        _excelImportService = excelImportService;
    }
    
    /// <summary>
    /// Get all employees - Admin only
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }
    
    /// <summary>
    /// Get employee by ID - Admin only
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        
        if (employee == null)
        {
            return NotFound(new { message = "Employee not found." });
        }
        
        return Ok(employee);
    }
    
    /// <summary>
    /// Create a new employee - Admin only
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Update an employee - Admin only
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.UpdateAsync(id, dto);
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Delete an employee - Admin only
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Generate PDF CV for an employee - Admin only
    /// </summary>
    [HttpGet("{id}/pdf")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GeneratePdf(int id)
    {
        try
        {
            var pdfBytes = await _pdfService.GenerateEmployeeCvAsync(id);
            return File(pdfBytes, "application/pdf", $"CV_Employee_{id}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Import employees from Excel file - Admin only
    /// </summary>
    [HttpPost("import")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded." });
        }
        
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (extension != ".xlsx" && extension != ".xls")
        {
            return BadRequest(new { message = "Invalid file format. Please upload an Excel file (.xlsx or .xls)." });
        }
        
        try
        {
            using var stream = file.OpenReadStream();
            var result = await _excelImportService.ImportEmployeesAsync(stream);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Get current employee's own information - Employee only (uses JWT to identify)
    /// </summary>
    [HttpGet("me")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetMyInfo()
    {
        var document = User.FindFirst("Document")?.Value;
        
        if (string.IsNullOrEmpty(document))
        {
            return Unauthorized(new { message = "Invalid token." });
        }
        
        var employee = await _employeeService.GetByDocumentAsync(document);
        
        if (employee == null)
        {
            return NotFound(new { message = "Employee not found." });
        }
        
        return Ok(employee);
    }
    
    /// <summary>
    /// Download own CV as PDF - Employee only (uses JWT to identify)
    /// </summary>
    [HttpGet("me/pdf")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DownloadMyPdf()
    {
        var document = User.FindFirst("Document")?.Value;
        
        if (string.IsNullOrEmpty(document))
        {
            return Unauthorized(new { message = "Invalid token." });
        }
        
        var employee = await _employeeService.GetByDocumentAsync(document);
        
        if (employee == null)
        {
            return NotFound(new { message = "Employee not found." });
        }
        
        try
        {
            var pdfBytes = await _pdfService.GenerateEmployeeCvAsync(employee.Id);
            return File(pdfBytes, "application/pdf", $"MiHojaDeVida.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}