using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByDocumentAsync(string document);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> DocumentExistsAsync(string document);
    Task<bool> EmailExistsAsync(string email);
    
    // For Dashboard
    Task<int> GetTotalCountAsync();
    Task<int> GetCountByStatusAsync(string statusName);
    Task<int> GetCountByDepartmentAsync(string departmentName);
    Task<int> GetCountByPositionAsync(string positionName);
}