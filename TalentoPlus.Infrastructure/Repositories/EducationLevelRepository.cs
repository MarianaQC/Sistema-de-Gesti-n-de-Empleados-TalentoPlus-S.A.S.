using Microsoft.EntityFrameworkCore;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;
using TalentoPlus.Infrastructure.Data;

namespace TalentoPlus.Infrastructure.Repositories;

public class EducationLevelRepository : IEducationLevelRepository
{
    private readonly ApplicationDbContext _context;
    
    public EducationLevelRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<EducationLevel>> GetAllAsync()
    {
        return await _context.EducationLevels.ToListAsync();
    }
    
    public async Task<EducationLevel?> GetByIdAsync(int id)
    {
        return await _context.EducationLevels.FindAsync(id);
    }
    
    public async Task<EducationLevel?> GetByNameAsync(string name)
    {
        return await _context.EducationLevels
            .FirstOrDefaultAsync(l => l.Name.ToLower() == name.ToLower());
    }
    
    public async Task<EducationLevel> CreateAsync(EducationLevel level)
    {
        _context.EducationLevels.Add(level);
        await _context.SaveChangesAsync();
        return level;
    }
}