using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TranslationManagementSystem.API.Models.DTOs
{
    public class ProjectCreateDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(10)]
        public string SourceLanguage { get; set; } = "en";

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string TargetLanguages { get; set; } = "[]";

        public DateTime? Deadline { get; set; }

        public string? Settings { get; set; }
    }
}