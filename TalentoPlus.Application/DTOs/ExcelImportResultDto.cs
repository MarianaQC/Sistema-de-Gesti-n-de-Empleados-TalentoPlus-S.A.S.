namespace TalentoPlus.Application.DTOs;

public class ExcelImportResultDto
{
    public bool Success { get; set; }
    public int TotalRows { get; set; }
    public int ImportedRows { get; set; }
    public int UpdatedRows { get; set; }
    public int ErrorRows { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}