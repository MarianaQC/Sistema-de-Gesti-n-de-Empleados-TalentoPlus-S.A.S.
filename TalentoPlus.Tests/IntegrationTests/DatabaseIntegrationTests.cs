using Microsoft.EntityFrameworkCore;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Infrastructure.Data;
using Xunit;

namespace TalentoPlus.Tests.IntegrationTests;

/// <summary>
/// Integration tests that verify database operations work correctly.
/// Uses in-memory database - no real MySQL needed.
/// </summary>
public class DatabaseIntegrationTests
{
    /// <summary>
    /// Creates a fresh in-memory database for each test.
    /// </summary>
    private ApplicationDbContext CreateTestDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }
    
    /// <summary>
    /// Integration Test 1: Verify we can save and retrieve a Department.
    /// </summary>
    [Fact]
    public async Task CanSaveDepartmentToDatabase()
    {
        // Arrange
        using var context = CreateTestDatabase();
        
        var department = new Department
        {
            Name = "Technology"
        };
        
        // Act
        context.Departments.Add(department);
        await context.SaveChangesAsync();
        
        // Assert
        var savedDepartment = await context.Departments
            .FirstOrDefaultAsync(d => d.Name == "Technology");
        
        Assert.NotNull(savedDepartment);
        Assert.Equal("Technology", savedDepartment.Name);
        Assert.True(savedDepartment.Id > 0);
    }
    
    /// <summary>
    /// Integration Test 2: Verify we can save an Employee with all relationships.
    /// </summary>
    [Fact]
    public async Task CanSaveEmployeeWithRelationships()
    {
        // Arrange
        using var context = CreateTestDatabase();
        
        // Create catalog data first (like seed data)
        var department = new Department { Name = "Human Resources" };
        var position = new Position { Name = "Manager" };
        var status = new EmployeeStatus { Name = "Activo" };
        var education = new EducationLevel { Name = "Profesional" };
        
        context.Departments.Add(department);
        context.Positions.Add(position);
        context.EmployeeStatuses.Add(status);
        context.EducationLevels.Add(education);
        await context.SaveChangesAsync();
        
        // Create employee with relationships
        var employee = new Employee
        {
            FullName = "Maria Garcia",
            Document = "1234567890",
            Email = "maria@test.com",
            Phone = "3001234567",
            Address = "Calle 123 #45-67",
            BirthDate = new DateTime(1990, 5, 15),
            HireDate = DateTime.Now,
            Salary = 5000000,
            ProfessionalProfile = "HR Professional with 5 years experience",
            DepartmentId = department.Id,
            PositionId = position.Id,
            EmployeeStatusId = status.Id,
            EducationLevelId = education.Id
        };
        
        // Act
        context.Employees.Add(employee);
        await context.SaveChangesAsync();
        
        // Assert
        var savedEmployee = await context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.EmployeeStatus)
            .Include(e => e.EducationLevel)
            .FirstOrDefaultAsync(e => e.Document == "1234567890");
        
        Assert.NotNull(savedEmployee);
        Assert.Equal("Maria Garcia", savedEmployee.FullName);
        Assert.Equal("Human Resources", savedEmployee.Department.Name);
        Assert.Equal("Manager", savedEmployee.Position.Name);
        Assert.Equal("Activo", savedEmployee.EmployeeStatus.Name);
        Assert.Equal("Profesional", savedEmployee.EducationLevel.Name);
    }
    
    /// <summary>
    /// Integration Test 3: Verify we can count employees by status (for Dashboard).
    /// </summary>
    [Fact]
    public async Task CanCountEmployeesByStatus()
    {
        // Arrange
        using var context = CreateTestDatabase();
        
        // Create catalog data
        var department = new Department { Name = "IT" };
        var position = new Position { Name = "Developer" };
        var activeStatus = new EmployeeStatus { Name = "Activo" };
        var vacationStatus = new EmployeeStatus { Name = "Vacaciones" };
        var education = new EducationLevel { Name = "Profesional" };
        
        context.Departments.Add(department);
        context.Positions.Add(position);
        context.EmployeeStatuses.AddRange(activeStatus, vacationStatus);
        context.EducationLevels.Add(education);
        await context.SaveChangesAsync();
        
        // Create 3 active employees
        for (int i = 1; i <= 3; i++)
        {
            context.Employees.Add(new Employee
            {
                FullName = $"Active Employee {i}",
                Document = $"100000000{i}",
                Email = $"active{i}@test.com",
                Phone = $"300000000{i}",
                Address = "Test Address",
                BirthDate = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now,
                Salary = 4000000,
                ProfessionalProfile = "Test Profile",
                DepartmentId = department.Id,
                PositionId = position.Id,
                EmployeeStatusId = activeStatus.Id,
                EducationLevelId = education.Id
            });
        }
        
        // Create 1 employee on vacation
        context.Employees.Add(new Employee
        {
            FullName = "Vacation Employee",
            Document = "2000000001",
            Email = "vacation@test.com",
            Phone = "3000000004",
            Address = "Test Address",
            BirthDate = DateTime.Now.AddYears(-25),
            HireDate = DateTime.Now,
            Salary = 3500000,
            ProfessionalProfile = "Test Profile",
            DepartmentId = department.Id,
            PositionId = position.Id,
            EmployeeStatusId = vacationStatus.Id,
            EducationLevelId = education.Id
        });
        
        await context.SaveChangesAsync();
        
        // Act - Count like the Dashboard would
        var activeCount = await context.Employees
            .CountAsync(e => e.EmployeeStatus.Name == "Activo");
        
        var vacationCount = await context.Employees
            .CountAsync(e => e.EmployeeStatus.Name == "Vacaciones");
        
        var totalCount = await context.Employees.CountAsync();
        
        // Assert
        Assert.Equal(3, activeCount);
        Assert.Equal(1, vacationCount);
        Assert.Equal(4, totalCount);
    }
}