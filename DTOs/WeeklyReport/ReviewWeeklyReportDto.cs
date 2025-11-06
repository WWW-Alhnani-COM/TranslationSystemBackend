// DTOs/WeeklyReport/ReviewWeeklyReportDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.WeeklyReport;

public class ReviewWeeklyReportDto
{
    [Required]
    public Guid ReportId { get; set; }

    [Required]
    [RegularExpression("^(Approved|Rejected)$")]
    public string Status { get; set; } = "Approved";

    public string? Feedback { get; set; }
}