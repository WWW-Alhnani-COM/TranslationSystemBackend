using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using System.Threading.Tasks;
using TranslationManagementSystem.API.Models.DTOs;
using InternshipManagement.API.DTOs.Auth;

namespace TranslationDB.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<User>> Register(RegisterDto registerDto);
        Task<ServiceResult<object>> Login(LoginDto loginDto);
        Task<bool> UserExists(string email);
        string GenerateJwtToken(User user);
        Task Register(RegisterDto registerDto);
    }
}