using Moq;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Services;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;
using Xunit;

namespace TalentoPlus.Tests.UnitTests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly EmployeeService _service;
    
    public EmployeeServiceTests()
    {
        _mockRepository = new Mock<IEmployeeRepository>();
        _service = new EmployeeService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsListOfEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                FullName = "John Doe",
                Document = "123456789",
                Email = "john@test.com",
                Department = new Department { Name = "IT" },
                Position = new Position { Name = "Developer" },
                EmployeeStatus = new EmployeeStatus { Name = "Active" },
                EducationLevel = new EducationLevel { Name = "Professional" }
            },
            new Employee
            {
                Id = 2,
                FullName = "Jane Doe",
                Document = "987654321",
                Email = "jane@test.com",
                Department = new Department { Name = "HR" },
                Position = new Position { Name = "Manager" },
                EmployeeStatus = new EmployeeStatus { Name = "Active" },
                EducationLevel = new EducationLevel { Name = "Master" }
            }
        };
        
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(employees);
        
        // Act
        var result = await _service.GetAllAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("John Doe", result[0].FullName);
        Assert.Equal("Jane Doe", result[1].FullName);
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenEmployeeExists_ReturnsEmployee()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FullName = "John Doe",
            Document = "123456789",
            Email = "john@test.com",
            Department = new Department { Name = "IT" },
            Position = new Position { Name = "Developer" },
            EmployeeStatus = new EmployeeStatus { Name = "Active" },
            EducationLevel = new EducationLevel { Name = "Professional" }
        };
        
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(employee);
        
        // Act
        var result = await _service.GetByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.FullName);
        Assert.Equal("123456789", result.Document);
    }
}