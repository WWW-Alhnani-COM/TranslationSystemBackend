// DTOs/AcademicSupervisor/UpdateInternshipStatusDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.AcademicSupervisor;

public class UpdateInternshipStatusDto
{
    [Required]
    public Guid InternshipId { get; set; }

    [Required]
    [RegularExpression("^(Approved|Rejected)$")]
    public string Status { get; set; } = "Approved";
}