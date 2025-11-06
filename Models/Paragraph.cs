using System.ComponentModel.DataAnnotations;
using TranslationDB.Enums;

namespace TranslationDB.Models
{
    public class Paragraph
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string OriginalText { get; set; }

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Created;

        public bool IsLocked { get; set; } = false;

        [StringLength(450)]
        public string? LockedBy { get; set; }

        public DateTime? LockedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual User? LockedByUser { get; set; }
        public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
    }
}