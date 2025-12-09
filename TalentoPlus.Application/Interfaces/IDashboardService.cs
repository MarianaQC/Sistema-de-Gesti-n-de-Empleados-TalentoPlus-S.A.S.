using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync();
}