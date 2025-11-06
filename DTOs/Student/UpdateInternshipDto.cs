// DTOs/Student/UpdateInternshipDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Student;

public class UpdateInternshipDto
{
    [Required]
    public Guid Id { get; set; }

    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; } // فقط إذا كان الطالب مسموح له بتعديل الحالة (نادرًا)
    public string? Description { get; set; }
    public string? AgreementDocumentUrl { get; set; }
}