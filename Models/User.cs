using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TranslationDB.Enums;

namespace TranslationDB.Models
{
    public class User
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z0-9_.]+$", ErrorMessage = "Invalid username format")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public UserType Role { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(30)]
        public string? PhoneNumber { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }

        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? ProfileImageUrl { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
        public virtual ICollection<Translation> Translations { get; set; } = new List<Translation>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Paragraph> LockedParagraphs { get; set; } = new List<Paragraph>();
    }
}
