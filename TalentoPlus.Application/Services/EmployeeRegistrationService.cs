using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;

namespace TalentoPlus.Application.Services;

public class EmployeeRegistrationService : IEmployeeRegistrationService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmailService _emailService;
    private readonly IEmployeeStatusRepository _statusRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IEducationLevelRepository _educationRepository;
    
    public EmployeeRegistrationService(
        IEmployeeRepository employeeRepository,
        IEmailService emailService,
        IEmployeeStatusRepository statusRepository,
        IPositionRepository positionRepository,
        IEducationLevelRepository educationRepository)
    {
        _employeeRepository = employeeRepository;
        _emailService = emailService;
        _statusRepository = statusRepository;
        _positionRepository = positionRepository;
        _educationRepository = educationRepository;
    }
    
    public async Task<EmployeeDto> RegisterAsync(EmployeeRegistrationDto dto)
    {
        // Validate document doesn't exist
        if (await _employeeRepository.DocumentExistsAsync(dto.Document))
        {
            throw new Exception("An employee with this document already exists.");
        }
        
        // Validate email doesn't exist
        if (await _employeeRepository.EmailExistsAsync(dto.Email))
        {
            throw new Exception("An employee with this email already exists.");
        }
        
        // Get default values for required fields
        var defaultStatus = await _statusRepository.GetByNameAsync("Activo");
        var defaultPosition = await _positionRepository.GetByNameAsync("Auxiliar");
        var defaultEducation = await _educationRepository.GetByNameAsync("Bachiller");
        
        var employee = new Employee
        {
            FullName = dto.FullName,
            Document = dto.Document,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
            HireDate = DateTime.UtcNow,
            Salary = 0, // Admin will set later
            ProfessionalProfile = "",
            Password = dto.Password,
            DepartmentId = dto.DepartmentId,
            PositionId = defaultPosition?.Id ?? 4,
            EmployeeStatusId = defaultStatus?.Id ?? 1,
            EducationLevelId = defaultEducation?.Id ?? 1
        };
        
        var created = await _employeeRepository.CreateAsync(employee);
        
        // Send welcome email
        try
        {
            await _emailService.SendWelcomeEmailAsync(dto.Email, dto.FullName);
        }
        catch (Exception)
        {
            // Log the error but don't fail the registration
        }
        
        // Get full employee data
        var result = await _employeeRepository.GetByIdAsync(created.Id);
        
        return new EmployeeDto
        {
            Id = result!.Id,
            FullName = result.FullName,
            Document = result.Document,
            Email = result.Email,
            Phone = result.Phone,
            Address = result.Address,
            BirthDate = result.BirthDate,
            HireDate = result.HireDate,
            Salary = result.Salary,
            ProfessionalProfile = result.ProfessionalProfile,
            DepartmentName = result.Department?.Name ?? "",
            PositionName = result.Position?.Name ?? "",
            StatusName = result.EmployeeStatus?.Name ?? "",
            EducationLevelName = result.EducationLevel?.Name ?? ""
        };
    }
}