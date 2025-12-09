using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Application.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto?> GetByDocumentAsync(string document);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task DeleteAsync(int id);
    Task<EmployeeDto?> ValidateLoginAsync(string document, string email);
}