using System.ComponentModel.DataAnnotations;

namespace TranslationDB.Models.DTOs;

public sealed class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_.]+$", ErrorMessage = "Username can contain only letters, numbers, underscores and periods.")]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string UserType { get; set; } = "DataEntry";

    [Phone, StringLength(30)]
    public string? PhoneNumber { get; set; }
}
