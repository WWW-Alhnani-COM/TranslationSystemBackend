// DTOs/Student/InternshipDto.cs
namespace InternshipManagement.API.DTOs.Student;

public class InternshipDto
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "PendingApproval"; // أو enum
    public string? Description { get; set; }
    public string? AgreementDocumentUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}