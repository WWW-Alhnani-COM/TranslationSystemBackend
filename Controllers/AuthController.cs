using InternshipManagement.API.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.Register(registerDto);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "User registered successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Registration failed: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _authService.Login(loginDto);
                if (!result.Success)
                    return HandleError(result.Message, 401);

                return HandleResult(result.Data, "Login successful");
            }
            catch (Exception ex)
            {
                return HandleError($"Login failed: {ex.Message}");
            }
        }
    }
}