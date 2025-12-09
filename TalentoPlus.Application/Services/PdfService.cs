using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TalentoPlus.Application.Interfaces;
using TalentoPlus.Domain.Interfaces;

namespace TalentoPlus.Application.Services;

public class PdfService : IPdfService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public PdfService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        
        // Configure QuestPDF license (free for community)
        QuestPDF.Settings.License = LicenseType.Community;
    }
    
    public async Task<byte[]> GenerateEmployeeCvAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }
        
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));
                
                page.Header().Element(header =>
                {
                    header.Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("HOJA DE VIDA")
                                .FontSize(24)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);
                            
                            column.Item().Text("TalentoPlus S.A.S.")
                                .FontSize(14)
                                .FontColor(Colors.Grey.Darken1);
                        });
                    });
                });
                
                page.Content().PaddingVertical(20).Column(column =>
                {
                    // Personal Information Section
                    column.Item().Element(element =>
                    {
                        element.Background(Colors.Blue.Lighten5)
                            .Padding(15)
                            .Column(col =>
                            {
                                col.Item().Text("DATOS PERSONALES")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(10).Row(row =>
                                {
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text($"Nombre Completo: {employee.FullName}").Bold();
                                        c.Item().Text($"Documento: {employee.Document}");
                                        c.Item().Text($"Fecha de Nacimiento: {employee.BirthDate:dd/MM/yyyy}");
                                    });
                                });
                            });
                    });
                    
                    column.Item().PaddingTop(15);
                    
                    // Contact Information Section
                    column.Item().Element(element =>
                    {
                        element.Background(Colors.Grey.Lighten4)
                            .Padding(15)
                            .Column(col =>
                            {
                                col.Item().Text("INFORMACIÓN DE CONTACTO")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(10).Column(c =>
                                {
                                    c.Item().Text($"Email: {employee.Email}");
                                    c.Item().Text($"Teléfono: {employee.Phone}");
                                    c.Item().Text($"Dirección: {employee.Address}");
                                });
                            });
                    });
                    
                    column.Item().PaddingTop(15);
                    
                    // Work Information Section
                    column.Item().Element(element =>
                    {
                        element.Background(Colors.Green.Lighten5)
                            .Padding(15)
                            .Column(col =>
                            {
                                col.Item().Text("INFORMACIÓN LABORAL")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(10).Column(c =>
                                {
                                    c.Item().Text($"Departamento: {employee.Department?.Name ?? "N/A"}");
                                    c.Item().Text($"Cargo: {employee.Position?.Name ?? "N/A"}");
                                    c.Item().Text($"Estado: {employee.EmployeeStatus?.Name ?? "N/A"}");
                                    c.Item().Text($"Fecha de Ingreso: {employee.HireDate:dd/MM/yyyy}");
                                    c.Item().Text($"Salario: ${employee.Salary:N2}");
                                });
                            });
                    });
                    
                    column.Item().PaddingTop(15);
                    
                    // Education Section
                    column.Item().Element(element =>
                    {
                        element.Background(Colors.Orange.Lighten5)
                            .Padding(15)
                            .Column(col =>
                            {
                                col.Item().Text("NIVEL EDUCATIVO")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(10).Text($"{employee.EducationLevel?.Name ?? "N/A"}");
                            });
                    });
                    
                    column.Item().PaddingTop(15);
                    
                    // Professional Profile Section
                    column.Item().Element(element =>
                    {
                        element.Background(Colors.Purple.Lighten5)
                            .Padding(15)
                            .Column(col =>
                            {
                                col.Item().Text("PERFIL PROFESIONAL")
                                    .FontSize(14)
                                    .Bold()
                                    .FontColor(Colors.Blue.Darken2);
                                
                                col.Item().PaddingTop(10).Text(employee.ProfessionalProfile);
                            });
                    });
                });
                
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Generado el ");
                    text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    text.Span(" - TalentoPlus S.A.S.");
                });
            });
        });
        
        return document.GeneratePdf();
    }
}