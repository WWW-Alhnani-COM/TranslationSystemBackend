using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.Register(registerDto);
        if (!result.Success)
            return HandleError(result.Message);

        return HandleResult(result.Data, "User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);
        if (!result.Success)
            return HandleError(result.Message, 401);

        return HandleResult(result.Data, "Login successful");
    }
}
