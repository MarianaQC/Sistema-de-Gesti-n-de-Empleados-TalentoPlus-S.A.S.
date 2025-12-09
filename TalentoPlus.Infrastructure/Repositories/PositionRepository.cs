using Microsoft.EntityFrameworkCore;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;
using TalentoPlus.Infrastructure.Data;

namespace TalentoPlus.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _context;
    
    public PositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Position>> GetAllAsync()
    {
        return await _context.Positions.ToListAsync();
    }
    
    public async Task<Position?> GetByIdAsync(int id)
    {
        return await _context.Positions.FindAsync(id);
    }
    
    public async Task<Position?> GetByNameAsync(string name)
    {
        return await _context.Positions
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }
    
    public async Task<Position> CreateAsync(Position position)
    {
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();
        return position;
    }
}