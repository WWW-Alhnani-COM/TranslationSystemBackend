using System.ComponentModel.DataAnnotations;
using TranslationDB.Models;
using TranslationDB.Enums;

namespace TranslationDB.Models
{
    public class Translation
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int ParagraphId { get; set; }

        [Required]
        [StringLength(10)]
        public string TargetLanguage { get; set; } = string.Empty;

        [Required]
        public string TranslatedText { get; set; } = string.Empty;

        [Required]
        public int Version { get; set; } = 1;

        [Required]
        public TranslationStatus Status { get; set; } = TranslationStatus.Draft;

        [Required]
        [StringLength(450)]
        public string TranslatorId { get; set; } = string.Empty;

        public DateTime TranslatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }

        // Navigation properties
        public virtual Paragraph Paragraph { get; set; } = new Paragraph();
        public virtual User Translator { get; set; } = new User();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}