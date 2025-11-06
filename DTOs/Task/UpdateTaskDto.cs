// DTOs/Task/UpdateTaskDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Task;

public class UpdateTaskDto
{
    [Required]
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; } // Pending, InProgress, Completed, Overdue
    public string? Priority { get; set; } // Low, Medium, High
    public string? SubmissionUrl { get; set; }
    public string? Feedback { get; set; }
}