// DTOs/WeeklyReport/CreateWeeklyReportDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.WeeklyReport;

public class CreateWeeklyReportDto
{
    [Required]
    public Guid InternshipId { get; set; }

    [Required]
    public int WeekNumber { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? Achievements { get; set; }
    public string? Challenges { get; set; }
    public string? NextWeekPlan { get; set; }
    public int HoursWorked { get; set; } = 0;
    public string? FileUrl { get; set; }
}