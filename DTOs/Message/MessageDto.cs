// DTOs/Message/MessageDto.cs
namespace InternshipManagement.API.DTOs.Message;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public Guid ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}