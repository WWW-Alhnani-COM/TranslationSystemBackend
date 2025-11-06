// DTOs/SiteSupervisor/InternshipForSupervisorDto.cs
namespace InternshipManagement.API.DTOs.SiteSupervisor;

public class InternshipForSupervisorDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "PendingApproval";
    public DateTime CreatedAt { get; set; }
}