using TalentoPlus.Application.DTOs;

namespace TalentoPlus.Application.Interfaces;

public interface IAiService
{
    Task<AiResponseDto> ProcessQueryAsync(string question);
}