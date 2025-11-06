// DTOs/Evaluation/CreateEvaluationDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Evaluation;

public class CreateEvaluationDto
{
    [Required]
    public Guid InternshipId { get; set; }

    [Range(1, 5)]
    public int TechnicalSkills { get; set; }

    [Range(1, 5)]
    public int Communication { get; set; }

    [Range(1, 5)]
    public int Professionalism { get; set; }

    [Range(1, 5)]
    public int ProblemSolving { get; set; }

    [Range(1, 5)]
    public int Teamwork { get; set; }

    public string? Comments { get; set; }
}