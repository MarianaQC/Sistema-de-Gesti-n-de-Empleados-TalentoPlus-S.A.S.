using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Interfaces;

namespace TalentoPlus.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public DashboardService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        return new DashboardDto
        {
            TotalEmployees = await _employeeRepository.GetTotalCountAsync(),
            EmployeesOnVacation = await _employeeRepository.GetCountByStatusAsync("Vacaciones"),
            ActiveEmployees = await _employeeRepository.GetCountByStatusAsync("Activo"),
            InactiveEmployees = await _employeeRepository.GetCountByStatusAsync("Inactivo")
        };
    }
}