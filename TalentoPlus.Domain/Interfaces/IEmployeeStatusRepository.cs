using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Domain.Interfaces;

public interface IEmployeeStatusRepository
{
    Task<List<EmployeeStatus>> GetAllAsync();
    Task<EmployeeStatus?> GetByIdAsync(int id);
    Task<EmployeeStatus?> GetByNameAsync(string name);
    Task<EmployeeStatus> CreateAsync(EmployeeStatus status);
}