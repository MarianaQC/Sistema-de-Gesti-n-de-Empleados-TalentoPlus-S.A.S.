using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;

namespace TalentoPlus.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(MapToDto).ToList();
    }
    
    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee != null ? MapToDto(employee) : null;
    }
    
    public async Task<EmployeeDto?> GetByDocumentAsync(string document)
    {
        var employee = await _employeeRepository.GetByDocumentAsync(document);
        return employee != null ? MapToDto(employee) : null;
    }
    
    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        // Check if document already exists
        if (await _employeeRepository.DocumentExistsAsync(dto.Document))
        {
            throw new Exception("An employee with this document already exists.");
        }
        
        // Check if email already exists
        if (await _employeeRepository.EmailExistsAsync(dto.Email))
        {
            throw new Exception("An employee with this email already exists.");
        }
        
        var employee = new Employee
        {
            FullName = dto.FullName,
            Document = dto.Document,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            ProfessionalProfile = dto.ProfessionalProfile,
            Password = dto.Password,
            DepartmentId = dto.DepartmentId,
            PositionId = dto.PositionId,
            EmployeeStatusId = dto.EmployeeStatusId,
            EducationLevelId = dto.EducationLevelId
        };
        
        var created = await _employeeRepository.CreateAsync(employee);
        
        // Reload with navigation properties
        var result = await _employeeRepository.GetByIdAsync(created.Id);
        return MapToDto(result!);
    }
    
    public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }
        
        employee.FullName = dto.FullName;
        employee.Phone = dto.Phone;
        employee.Address = dto.Address;
        employee.Salary = dto.Salary;
        employee.ProfessionalProfile = dto.ProfessionalProfile;
        employee.DepartmentId = dto.DepartmentId;
        employee.PositionId = dto.PositionId;
        employee.EmployeeStatusId = dto.EmployeeStatusId;
        employee.EducationLevelId = dto.EducationLevelId;
        
        await _employeeRepository.UpdateAsync(employee);
        
        // Reload with navigation properties
        var result = await _employeeRepository.GetByIdAsync(id);
        return MapToDto(result!);
    }
    
    public async Task DeleteAsync(int id)
    {
        if (!await _employeeRepository.ExistsAsync(id))
        {
            throw new Exception("Employee not found.");
        }
        
        await _employeeRepository.DeleteAsync(id);
    }
    
    public async Task<EmployeeDto?> ValidateLoginAsync(string document, string email)
    {
        var employee = await _employeeRepository.GetByDocumentAsync(document);
        
        if (employee != null && employee.Email.ToLower() == email.ToLower())
        {
            return MapToDto(employee);
        }
        
        return null;
    }
    
    private EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Document = employee.Document,
            Email = employee.Email,
            Phone = employee.Phone,
            Address = employee.Address,
            BirthDate = employee.BirthDate,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            ProfessionalProfile = employee.ProfessionalProfile,
            DepartmentName = employee.Department?.Name ?? "",
            PositionName = employee.Position?.Name ?? "",
            StatusName = employee.EmployeeStatus?.Name ?? "",
            EducationLevelName = employee.EducationLevel?.Name ?? ""
        };
    }
}