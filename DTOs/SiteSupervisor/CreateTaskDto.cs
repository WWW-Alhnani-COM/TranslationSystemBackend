// DTOs/SiteSupervisor/CreateTaskDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.SiteSupervisor;

public class CreateTaskDto
{
    [Required]
    public Guid InternshipId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Priority { get; set; } = "Medium";
}