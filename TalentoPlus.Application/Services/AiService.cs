using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TalentoPlus.Application.DTOs;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Interfaces;

namespace TalentoPlus.Application.Services;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IEmployeeRepository _employeeRepository;
    
    public AiService(
        HttpClient httpClient,
        IConfiguration configuration,
        IEmployeeRepository employeeRepository)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _employeeRepository = employeeRepository;
    }
    
    public async Task<AiResponseDto> ProcessQueryAsync(string question)
    {
        try
        {
            // First, use AI to understand the question and get the query type
            var queryType = await AnalyzeQuestionWithAiAsync(question);
            
            // Then execute the appropriate query based on AI interpretation
            var answer = await ExecuteQueryAsync(queryType, question);
            
            return new AiResponseDto
            {
                Question = question,
                Answer = answer,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new AiResponseDto
            {
                Question = question,
                Answer = $"Error processing query: {ex.Message}",
                Success = false
            };
        }
    }
    
    private async Task<string> AnalyzeQuestionWithAiAsync(string question)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        
        if (string.IsNullOrEmpty(apiKey))
        {
            // If no API key, use simple keyword matching
            return AnalyzeQuestionLocally(question);
        }
        
        try
        {
            var prompt = $@"
            Analyze this question about an employee management system and classify it.
            Question: {question}
            
            Return ONLY one of these categories:
            - TOTAL_EMPLOYEES (questions about total count)
            - STATUS_COUNT (questions about employee status like active, inactive, vacation)
            - DEPARTMENT_COUNT (questions about employees in a department)
            - POSITION_COUNT (questions about employees in a position/role)
            - UNKNOWN (if you can't classify)
            
            Also extract the relevant filter value if present (like department name, status, or position).
            Format: CATEGORY|FILTER_VALUE
            Example: DEPARTMENT_COUNT|Tecnología
            ";
            
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };
            
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}",
                content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonDocument.Parse(responseJson);
                var text = result.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();
                
                return text ?? AnalyzeQuestionLocally(question);
            }
        }
        catch
        {
            // Fall back to local analysis
        }
        
        return AnalyzeQuestionLocally(question);
    }
    
    private string AnalyzeQuestionLocally(string question)
    {
        var lowerQuestion = question.ToLower();
        
        // Check for total employees
        if (lowerQuestion.Contains("total") || 
            (lowerQuestion.Contains("cuántos") && lowerQuestion.Contains("empleados") && !lowerQuestion.Contains("departamento") && !lowerQuestion.Contains("estado")))
        {
            return "TOTAL_EMPLOYEES|";
        }
        
        // Check for status queries
        if (lowerQuestion.Contains("vacaciones"))
            return "STATUS_COUNT|Vacaciones";
        if (lowerQuestion.Contains("activo"))
            return "STATUS_COUNT|Activo";
        if (lowerQuestion.Contains("inactivo"))
            return "STATUS_COUNT|Inactivo";
        if (lowerQuestion.Contains("licencia"))
            return "STATUS_COUNT|Licencia";
        
        // Check for department queries
        if (lowerQuestion.Contains("tecnología") || lowerQuestion.Contains("tecnologia"))
            return "DEPARTMENT_COUNT|Tecnología";
        if (lowerQuestion.Contains("recursos humanos") || lowerQuestion.Contains("rrhh"))
            return "DEPARTMENT_COUNT|Recursos Humanos";
        if (lowerQuestion.Contains("finanzas"))
            return "DEPARTMENT_COUNT|Finanzas";
        if (lowerQuestion.Contains("marketing"))
            return "DEPARTMENT_COUNT|Marketing";
        if (lowerQuestion.Contains("operaciones"))
            return "DEPARTMENT_COUNT|Operaciones";
        
        // Check for position queries
        if (lowerQuestion.Contains("auxiliar"))
            return "POSITION_COUNT|Auxiliar";
        if (lowerQuestion.Contains("analista"))
            return "POSITION_COUNT|Analista";
        if (lowerQuestion.Contains("coordinador"))
            return "POSITION_COUNT|Coordinador";
        if (lowerQuestion.Contains("gerente"))
            return "POSITION_COUNT|Gerente";
        if (lowerQuestion.Contains("director"))
            return "POSITION_COUNT|Director";
        
        return "UNKNOWN|";
    }
    
    private async Task<string> ExecuteQueryAsync(string queryType, string originalQuestion)
    {
        var parts = queryType.Split('|');
        var category = parts[0];
        var filter = parts.Length > 1 ? parts[1] : "";
        
        switch (category)
        {
            case "TOTAL_EMPLOYEES":
                var total = await _employeeRepository.GetTotalCountAsync();
                return $"El número total de empleados registrados es: {total}";
                
            case "STATUS_COUNT":
                var statusCount = await _employeeRepository.GetCountByStatusAsync(filter);
                return $"Hay {statusCount} empleados con estado '{filter}'.";
                
            case "DEPARTMENT_COUNT":
                var deptCount = await _employeeRepository.GetCountByDepartmentAsync(filter);
                return $"Hay {deptCount} empleados en el departamento de '{filter}'.";
                
            case "POSITION_COUNT":
                var posCount = await _employeeRepository.GetCountByPositionAsync(filter);
                return $"Hay {posCount} empleados con el cargo de '{filter}'.";
                
            default:
                return "Lo siento, no pude entender la pregunta. Por favor, intente preguntar sobre: total de empleados, empleados por estado (activo, inactivo, vacaciones), empleados por departamento, o empleados por cargo.";
        }
    }
}