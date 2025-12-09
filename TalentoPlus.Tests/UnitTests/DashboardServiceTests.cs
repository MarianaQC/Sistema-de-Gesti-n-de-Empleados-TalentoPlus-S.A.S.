using Moq;
using TalentoPlus.Application.Services;
using TalentoPlus.Domain.Interfaces;
using Xunit;

namespace TalentoPlus.Tests.UnitTests;

public class DashboardServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly DashboardService _service;
    
    public DashboardServiceTests()
    {
        _mockRepository = new Mock<IEmployeeRepository>();
        _service = new DashboardService(_mockRepository.Object);
    }
    
    [Fact]
    public async Task GetDashboardDataAsync_ReturnsCorrectCounts()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetTotalCountAsync()).ReturnsAsync(100);
        _mockRepository.Setup(r => r.GetCountByStatusAsync("Vacaciones")).ReturnsAsync(10);
        _mockRepository.Setup(r => r.GetCountByStatusAsync("Activo")).ReturnsAsync(80);
        _mockRepository.Setup(r => r.GetCountByStatusAsync("Inactivo")).ReturnsAsync(10);
        
        // Act
        var result = await _service.GetDashboardDataAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.TotalEmployees);
        Assert.Equal(10, result.EmployeesOnVacation);
        Assert.Equal(80, result.ActiveEmployees);
        Assert.Equal(10, result.InactiveEmployees);
    }
}