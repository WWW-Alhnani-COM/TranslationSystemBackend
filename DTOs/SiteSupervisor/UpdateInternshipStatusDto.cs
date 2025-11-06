// DTOs/SiteSupervisor/UpdateInternshipStatusDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.SiteSupervisor;

public class UpdateInternshipStatusDto
{
    [Required]
    public Guid InternshipId { get; set; }

    [Required]
    [RegularExpression("^(Approved|Rejected)$")]
    public string Status { get; set; } = "Approved";
}

