namespace TalentoPlus.Application.DTOs;

public class AiQueryDto
{
    public string Question { get; set; } = string.Empty;
}

public class AiResponseDto
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public bool Success { get; set; }
}