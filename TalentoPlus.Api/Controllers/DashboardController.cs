using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;

namespace TalentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IAiService _aiService;
    
    public DashboardController(
        IDashboardService dashboardService,
        IAiService aiService)
    {
        _dashboardService = dashboardService;
        _aiService = aiService;
    }
    
    /// <summary>
    /// Get dashboard data with employee statistics - Admin only
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetDashboardData()
    {
        var data = await _dashboardService.GetDashboardDataAsync();
        return Ok(data);
    }
    
    /// <summary>
    /// Process AI query about employees - Admin only
    /// </summary>
    [HttpPost("ai-query")]
    public async Task<IActionResult> ProcessAiQuery([FromBody] AiQueryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Question))
        {
            return BadRequest(new { message = "Question cannot be empty." });
        }
        
        var response = await _aiService.ProcessQueryAsync(dto.Question);
        return Ok(response);
    }
}