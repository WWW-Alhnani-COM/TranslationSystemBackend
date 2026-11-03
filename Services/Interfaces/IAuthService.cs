using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<User>> Register(RegisterDto registerDto);
    Task<ServiceResult<object>> Login(LoginDto loginDto);
    Task<bool> UserExists(string email);
    string GenerateJwtToken(User user);
}
