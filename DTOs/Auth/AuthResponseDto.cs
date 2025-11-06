// DTOs/Auth/AuthResponseDto.cs
namespace InternshipManagement.API.DTOs.Auth;

public class AuthResponseDto
{
    // ⚠️ في حالة استخدام JWT Token (كما في Swagger)
    public string Token { get; set; } = string.Empty; // سيتم إزالته لاحقًا إذا استخدمت Cookies

    // ⚠️ في حالة استخدام Cookies، فقط هذه الحقول مهمة
    public string Role { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}