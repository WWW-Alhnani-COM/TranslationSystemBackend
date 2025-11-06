// DTOs/Student/CreateInternshipDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Student;

public class CreateInternshipDto
{
    [Required]
    public string CompanyName { get; set; } = string.Empty;

    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? Description { get; set; }
    public string? AgreementDocumentUrl { get; set; }
}