using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TranslationDB.Data;
using TranslationDB.Enums;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services;

public class AuthService : BaseService, IAuthService
{
    private static readonly HashSet<UserType> SelfRegistrationRoles =
    [
        UserType.DataEntry,
        UserType.Translator,
        UserType.Reviewer
    ];

    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ServiceResult<User>> Register(RegisterDto registerDto)
    {
        var email = registerDto.Email.Trim().ToLowerInvariant();
        var username = registerDto.Username.Trim();

        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == email))
            return Error<User>("An account with this email already exists.");

        if (await _context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
            return Error<User>("This username is already in use.");

        if (!Enum.TryParse<UserType>(registerDto.UserType, true, out var role) ||
            !SelfRegistrationRoles.Contains(role))
        {
            return Error<User>("Invalid user type. Self-registration is limited to DataEntry, Translator, and Reviewer.");
        }

        CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = username,
            Email = email,
            Role = role,
            FullName = $"{registerDto.FirstName.Trim()} {registerDto.LastName.Trim()}".Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(registerDto.PhoneNumber)
                ? null
                : registerDto.PhoneNumber.Trim(),
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Success(user, "User registered successfully");
    }

    public async Task<ServiceResult<object>> Login(LoginDto loginDto)
    {
        var email = loginDto.Email.Trim().ToLowerInvariant();
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email && u.IsActive);

        if (user?.PasswordHash is not { Length: > 0 } ||
            user.PasswordSalt is not { Length: > 0 } ||
            !VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Error<object>("Invalid email or password");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var result = new
        {
            Token = GenerateJwtToken(user),
            User = new
            {
                user.Id,
                user.UserName,
                user.Email,
                Role = user.Role.ToString(),
                user.FullName,
                user.ProfileImageUrl
            }
        };

        return Success<object>(result, "Login successful");
    }

    public Task<bool> UserExists(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
    }

    public string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT signing key is not configured.");
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT audience is not configured.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("FullName", user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    private static void CreatePasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt)
    {
        passwordSalt = RandomNumberGenerator.GetBytes(16);
        passwordHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            passwordSalt,
            iterations: 210_000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32);
    }

    private static bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            passwordSalt,
            iterations: 210_000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32);

        return CryptographicOperations.FixedTimeEquals(computedHash, passwordHash);
    }
}
