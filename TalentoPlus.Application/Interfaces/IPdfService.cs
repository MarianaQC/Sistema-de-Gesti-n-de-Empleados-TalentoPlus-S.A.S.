namespace TalentoPlus.Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateEmployeeCvAsync(int employeeId);
}