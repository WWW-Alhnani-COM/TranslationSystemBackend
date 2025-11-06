using System.ComponentModel.DataAnnotations;
using TranslationDB.Enums;
using TranslationDB.Models;
namespace TranslationDB.Models
{
    public class Review
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(450)]
        public string TranslationId { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string ReviewerId { get; set; } = string.Empty;

        public string? ReviewedText { get; set; }
        public string? Comments { get; set; }

        [Range(1, 5)]
        public byte? Rating { get; set; }

        [Required]
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Translation Translation { get; set; } = new Translation();
        public virtual User Reviewer { get; set; } = new User();
    }
}