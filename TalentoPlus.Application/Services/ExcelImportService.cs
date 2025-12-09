using OfficeOpenXml;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Entities;
using TalentoPlus.Domain.Interfaces;

namespace TalentoPlus.Application.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IEmployeeStatusRepository _statusRepository;
    private readonly IEducationLevelRepository _educationRepository;
    
    public ExcelImportService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        IPositionRepository positionRepository,
        IEmployeeStatusRepository statusRepository,
        IEducationLevelRepository educationRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
        _statusRepository = statusRepository;
        _educationRepository = educationRepository;
    }
    
    public async Task<ExcelImportResultDto> ImportEmployeesAsync(Stream fileStream)
    {
        var result = new ExcelImportResultDto { Success = true };
        
        // Set EPPlus license (free for non-commercial)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage(fileStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        
        if (worksheet == null)
        {
            result.Success = false;
            result.Errors.Add("No worksheet found in the Excel file.");
            return result;
        }
        
        var rowCount = worksheet.Dimension?.Rows ?? 0;
        result.TotalRows = rowCount - 1; // Excluding header
        
        // Start from row 2 (skip header)
        for (int row = 2; row <= rowCount; row++)
        {
            try
            {
                // --- INICIO DEL CÓDIGO CORREGIDO PARA EL MAPEADO ---
                
                // Columna 1: Documento
                var document = worksheet.Cells[row, 1].Text?.Trim();

                // Columna 2: Nombres
                var names = worksheet.Cells[row, 2].Text?.Trim();
                // Columna 3: Apellidos
                var surnames = worksheet.Cells[row, 3].Text?.Trim();
                // Concatenamos Nombres y Apellidos para FullName
                var fullName = $"{names} {surnames}".Trim();

                // Columna 4: FechaNacimiento -> birthDateText
                var birthDateText = worksheet.Cells[row, 4].Text?.Trim(); 

                // Columna 5: Direccion -> address
                var address = worksheet.Cells[row, 5].Text?.Trim(); 

                // Columna 6: Telefono -> phone
                var phone = worksheet.Cells[row, 6].Text?.Trim(); 

                // Columna 7: Email
                var email = worksheet.Cells[row, 7].Text?.Trim(); 

                // Columna 8: Cargo -> positionName
                var positionName = worksheet.Cells[row, 8].Text?.Trim(); 

                // Columna 9: Salario -> salaryText
                var salaryText = worksheet.Cells[row, 9].Text?.Trim(); 

                // Columna 10: FechaIngreso -> hireDateText
                var hireDateText = worksheet.Cells[row, 10].Text?.Trim(); 

                // Columna 11: Estado -> statusName
                var statusName = worksheet.Cells[row, 11].Text?.Trim(); 

                // Columna 12: NivelEducativo -> educationName
                var educationName = worksheet.Cells[row, 12].Text?.Trim(); 

                // Columna 13: PerfilProfesional -> professionalProfile
                var professionalProfile = worksheet.Cells[row, 13].Text?.Trim(); 

                // Columna 14: Departamento -> departmentName (Ahora usa la Columna 14)
                var departmentName = worksheet.Cells[row, 14].Text?.Trim();
                
                // --- FIN DEL CÓDIGO CORREGIDO ---
                
                // Validate required fields
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(document) || string.IsNullOrEmpty(email))
                {
                    result.Errors.Add($"Row {row}: Missing required fields (FullName, Document, or Email).");
                    result.ErrorRows++;
                    continue;
                }
                
                // Parse dates
                DateTime birthDate = DateTime.Now.AddYears(-30);
                DateTime hireDate = DateTime.Now;
                
                if (!string.IsNullOrEmpty(birthDateText))
                {
                    DateTime.TryParse(birthDateText, out birthDate);
                }
                
                if (!string.IsNullOrEmpty(hireDateText))
                {
                    DateTime.TryParse(hireDateText, out hireDate);
                }
                
                // Parse salary
                decimal salary = 0;
                if (!string.IsNullOrEmpty(salaryText))
                {
                    decimal.TryParse(salaryText.Replace(",", "").Replace("$", ""), out salary);
                }
                
                // Get or create related entities
                var department = await GetOrCreateDepartmentAsync(departmentName ?? "Sin Departamento");
                var position = await GetOrCreatePositionAsync(positionName ?? "Sin Cargo");
                var status = await GetOrCreateStatusAsync(statusName ?? "Activo");
                var education = await GetOrCreateEducationAsync(educationName ?? "Bachiller");
                
                // Check if employee exists
                var existingEmployee = await _employeeRepository.GetByDocumentAsync(document);
                
                if (existingEmployee != null)
                {
                    // Update existing employee
                    existingEmployee.FullName = fullName;
                    existingEmployee.Email = email;
                    existingEmployee.Phone = phone ?? "";
                    existingEmployee.Address = address ?? "";
                    existingEmployee.BirthDate = birthDate;
                    existingEmployee.HireDate = hireDate;
                    existingEmployee.Salary = salary;
                    existingEmployee.ProfessionalProfile = professionalProfile ?? "";
                    existingEmployee.DepartmentId = department.Id;
                    existingEmployee.PositionId = position.Id;
                    existingEmployee.EmployeeStatusId = status.Id;
                    existingEmployee.EducationLevelId = education.Id;
                    
                    await _employeeRepository.UpdateAsync(existingEmployee);
                    result.UpdatedRows++;
                }
                else
                {
                    // Create new employee
                    var newEmployee = new Employee
                    {
                        FullName = fullName,
                        Document = document,
                        Email = email,
                        Phone = phone ?? "",
                        Address = address ?? "",
                        BirthDate = birthDate,
                        HireDate = hireDate,
                        Salary = salary,
                        ProfessionalProfile = professionalProfile ?? "",
                        Password = document, // Default password is the document
                        DepartmentId = department.Id,
                        PositionId = position.Id,
                        EmployeeStatusId = status.Id,
                        EducationLevelId = education.Id
                    };
                    
                    await _employeeRepository.CreateAsync(newEmployee);
                    result.ImportedRows++;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Row {row}: An error occurred while saving the entity changes. See the inner exception for details. (Error: {ex.Message})");
                result.ErrorRows++;
            }
        }
        
        if (result.ErrorRows > 0 && result.ImportedRows == 0 && result.UpdatedRows == 0)
        {
            result.Success = false;
        }
        
        return result;
    }
    
    private async Task<Department> GetOrCreateDepartmentAsync(string name)
    {
        var department = await _departmentRepository.GetByNameAsync(name);
        if (department != null) return department;
        
        return await _departmentRepository.CreateAsync(new Department { Name = name });
    }
    
    private async Task<Position> GetOrCreatePositionAsync(string name)
    {
        var position = await _positionRepository.GetByNameAsync(name);
        if (position != null) return position;
        
        return await _positionRepository.CreateAsync(new Position { Name = name });
    }
    
    private async Task<EmployeeStatus> GetOrCreateStatusAsync(string name)
    {
        var status = await _statusRepository.GetByNameAsync(name);
        if (status != null) return status;
        
        return await _statusRepository.CreateAsync(new EmployeeStatus { Name = name });
    }
    
    private async Task<EducationLevel> GetOrCreateEducationAsync(string name)
    {
        var education = await _educationRepository.GetByNameAsync(name);
        if (education != null) return education;
        
        return await _educationRepository.CreateAsync(new EducationLevel { Name = name });
    }
}