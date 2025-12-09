using Microsoft.EntityFrameworkCore;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;
using TalentoPlus.Infrastructure.Data;

namespace TalentoPlus.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    
    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.EmployeeStatus)
            .Include(e => e.EducationLevel)
            .ToListAsync();
    }
    
    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.EmployeeStatus)
            .Include(e => e.EducationLevel)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<Employee?> GetByDocumentAsync(string document)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.EmployeeStatus)
            .Include(e => e.EducationLevel)
            .FirstOrDefaultAsync(e => e.Document == document);
    }
    
    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.EmployeeStatus)
            .Include(e => e.EducationLevel)
            .FirstOrDefaultAsync(e => e.Email == email);
    }
    
    public async Task<Employee> CreateAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    
    public async Task<Employee> UpdateAsync(Employee employee)
    {
        employee.UpdatedAt = DateTime.UtcNow;
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    
    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
    
    public async Task<bool> DocumentExistsAsync(string document)
    {
        return await _context.Employees.AnyAsync(e => e.Document == document);
    }
    
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Employees.AnyAsync(e => e.Email == email);
    }
    
    // Dashboard methods
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Employees.CountAsync();
    }
    
    public async Task<int> GetCountByStatusAsync(string statusName)
    {
        return await _context.Employees
            .Include(e => e.EmployeeStatus)
            .CountAsync(e => e.EmployeeStatus != null && 
                           e.EmployeeStatus.Name.ToLower().Contains(statusName.ToLower()));
    }
    
    public async Task<int> GetCountByDepartmentAsync(string departmentName)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .CountAsync(e => e.Department != null && 
                           e.Department.Name.ToLower().Contains(departmentName.ToLower()));
    }
    
    public async Task<int> GetCountByPositionAsync(string positionName)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .CountAsync(e => e.Position != null && 
                           e.Position.Name.ToLower().Contains(positionName.ToLower()));
    }
}