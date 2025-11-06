// DTOs/Message/CreateMessageDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Message;

public class CreateMessageDto
{
    [Required]
    public Guid ReceiverId { get; set; }

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;
}