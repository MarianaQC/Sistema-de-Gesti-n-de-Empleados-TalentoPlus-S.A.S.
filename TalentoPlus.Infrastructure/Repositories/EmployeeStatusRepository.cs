using Microsoft.EntityFrameworkCore;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;
using TalentoPlus.Infrastructure.Data;

namespace TalentoPlus.Infrastructure.Repositories;

public class EmployeeStatusRepository : IEmployeeStatusRepository
{
    private readonly ApplicationDbContext _context;
    
    public EmployeeStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<EmployeeStatus>> GetAllAsync()
    {
        return await _context.EmployeeStatuses.ToListAsync();
    }
    
    public async Task<EmployeeStatus?> GetByIdAsync(int id)
    {
        return await _context.EmployeeStatuses.FindAsync(id);
    }
    
    public async Task<EmployeeStatus?> GetByNameAsync(string name)
    {
        return await _context.EmployeeStatuses
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
    }
    
    public async Task<EmployeeStatus> CreateAsync(EmployeeStatus status)
    {
        _context.EmployeeStatuses.Add(status);
        await _context.SaveChangesAsync();
        return status;
    }
}