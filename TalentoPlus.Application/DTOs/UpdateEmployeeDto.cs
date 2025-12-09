namespace TalentoPlus.Application.DTOs;

public class UpdateEmployeeDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string ProfessionalProfile { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public int EmployeeStatusId { get; set; }
    public int EducationLevelId { get; set; }
}