// DTOs/WeeklyReport/UpdateWeeklyReportDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.WeeklyReport;

public class UpdateWeeklyReportDto
{
    [Required]
    public Guid Id { get; set; }

    public string? Achievements { get; set; }
    public string? Challenges { get; set; }
    public string? NextWeekPlan { get; set; }
    public int HoursWorked { get; set; }
    public string? FileUrl { get; set; }
    public string? Status { get; set; } // فقط إذا كان مسموحًا للطالب بتعديل الحالة (نادرًا)
}