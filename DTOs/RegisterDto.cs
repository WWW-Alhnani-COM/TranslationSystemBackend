// DTOs/Auth/RegisterDto.cs
using System.ComponentModel.DataAnnotations;

namespace InternshipManagement.API.DTOs.Auth;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [RoleValidation]
    public string Role { get; set; } = "Student";

    public string? Phone { get; set; }
}

public class RoleValidationAttribute : ValidationAttribute
{
    private static readonly string[] AllowedRoles = { "Student", "SiteSupervisor", "AcademicSupervisor", "Admin" };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var role = value as string;
        if (string.IsNullOrWhiteSpace(role) || !AllowedRoles.Contains(role))
        {
            return new ValidationResult("Role must be one of: Student, SiteSupervisor, AcademicSupervisor, Admin.");
        }
        return ValidationResult.Success;
    }
}