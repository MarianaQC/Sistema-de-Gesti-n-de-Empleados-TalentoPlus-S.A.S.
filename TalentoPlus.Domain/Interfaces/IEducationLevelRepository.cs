using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Domain.Interfaces;

public interface IEducationLevelRepository
{
    Task<List<EducationLevel>> GetAllAsync();
    Task<EducationLevel?> GetByIdAsync(int id);
    Task<EducationLevel?> GetByNameAsync(string name);
    Task<EducationLevel> CreateAsync(EducationLevel level);
}