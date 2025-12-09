namespace TalentoPlus.Application.DTOs;

public class EmployeeRegistrationDto
{
    public string FullName { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Password { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}