// DTOs/Task/TaskDto.cs
namespace InternshipManagement.API.DTOs.Task;

public class TaskDto
{
    public Guid Id { get; set; }
    public Guid InternshipId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Overdue
    public string Priority { get; set; } = "Medium"; // Low, Medium, High
    public string? SubmissionUrl { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string? Feedback { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}