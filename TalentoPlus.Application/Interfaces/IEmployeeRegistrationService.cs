using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Application.Interfaces;

public interface IEmployeeRegistrationService
{
    Task<EmployeeDto> RegisterAsync(EmployeeRegistrationDto dto);
}