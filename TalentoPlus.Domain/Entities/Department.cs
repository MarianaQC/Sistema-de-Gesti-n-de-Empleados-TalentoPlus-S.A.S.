namespace TalentoPlus.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Navigation property
    public List<Employee> Employees { get; set; } = new List<Employee>();
}