using System.ComponentModel.DataAnnotations;
using TranslationDB.Models;

namespace TranslationDB.Models
{
    public class ProjectAssignment
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string AssignmentType { get; set; } = string.Empty; // "Translator", "Reviewer"

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Project Project { get; set; } = new Project();
        public virtual User User { get; set; } = new User();
    }
}