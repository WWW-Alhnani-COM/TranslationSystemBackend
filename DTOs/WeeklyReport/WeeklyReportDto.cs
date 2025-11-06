// DTOs/WeeklyReport/WeeklyReportDto.cs
namespace InternshipManagement.API.DTOs.WeeklyReport;

public class WeeklyReportDto
{
    public Guid Id { get; set; }
    public Guid InternshipId { get; set; }
    public int WeekNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Achievements { get; set; }
    public string? Challenges { get; set; }
    public string? NextWeekPlan { get; set; }
    public int HoursWorked { get; set; }
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected
    public string? FileUrl { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Feedback { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}