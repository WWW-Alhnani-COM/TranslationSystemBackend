using System.ComponentModel.DataAnnotations;
using TranslationDB.Models;

namespace TranslationDB.Models
{
    public class Notification
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [StringLength(450)]
        public string? RelatedEntityId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = new User();
    }
}