using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Application.Interfaces;

public interface IExcelImportService
{
    Task<ExcelImportResultDto> ImportEmployeesAsync(Stream fileStream);
}