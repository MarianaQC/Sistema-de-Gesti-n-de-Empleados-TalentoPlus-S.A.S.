namespace TalentoPlus.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public string ProfessionalProfile { get; set; } = string.Empty;
    
    // Password for API login (simple approach for beginners)
    public string Password { get; set; } = string.Empty;
    
    // Foreign Keys
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public int EmployeeStatusId { get; set; }
    public int EducationLevelId { get; set; }
    
    // Navigation Properties
    public Department? Department { get; set; }
    public Position? Position { get; set; }
    public EmployeeStatus? EmployeeStatus { get; set; }
    public EducationLevel? EducationLevel { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}