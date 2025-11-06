using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TranslationDB.Enums;

namespace TranslationDB.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [StringLength(10)]
        public string SourceLanguage { get; set; } = "en";

        [Required]
        [Column(TypeName = "json")]
        public string TargetLanguages { get; set; } = "[]";

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Created;

        [Required]
        public string CreatedById { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? Deadline { get; set; }

        [Column(TypeName = "json")]
        public string? Settings { get; set; }

        // Navigation properties
        public virtual User Creator { get; set; } = new User();
        public virtual ICollection<Paragraph> Paragraphs { get; set; } = new List<Paragraph>();
        public virtual ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();
    }
}