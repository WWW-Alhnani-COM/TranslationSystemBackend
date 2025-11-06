// DTOs/Evaluation/EvaluationDto.cs
namespace InternshipManagement.API.DTOs.Evaluation;

public class EvaluationDto
{
    public Guid Id { get; set; }
    public Guid InternshipId { get; set; }
    public Guid EvaluatorId { get; set; }
    public string EvaluatorType { get; set; } = string.Empty; // "SiteSupervisor" or "AcademicSupervisor"
    public int TechnicalSkills { get; set; }
    public int Communication { get; set; }
    public int Professionalism { get; set; }
    public int ProblemSolving { get; set; }
    public int Teamwork { get; set; }
    public decimal OverallRating { get; set; }
    public string? Comments { get; set; }
    public DateTime CreatedAt { get; set; }
}