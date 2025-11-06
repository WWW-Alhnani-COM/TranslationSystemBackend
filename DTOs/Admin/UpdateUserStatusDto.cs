// DTOs/Admin/UpdateUserStatusDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Admin;

public class UpdateUserStatusDto
{
    [Required]
    public Guid UserId { get; set; }

    public bool IsActive { get; set; } = true;
}