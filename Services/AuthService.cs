using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResult<User>> Register(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                if (await UserExists(registerDto.Email))
                {
                    return Error<User>("User with this email already exists");
                }

                // Create password hash
                CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    Role = registerDto.Role,
                    FullName = registerDto.FullName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Success(user, "User registered successfully");
            }
            catch (Exception ex)
            {
                return Error<User>($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ServiceResult<object>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

                if (user == null)
                {
                    return Error<object>("Invalid email or password");
                }

                if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return Error<object>("Invalid email or password");
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                var result = new
                {
                    Token = token,
                    User = new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.Role,
                        user.FullName,
                        user.ProfileImageUrl
                    }
                };

                return Success<object>(result, "Login successful");
            }
            catch (Exception ex)
            {
                return Error<object>($"Login failed: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration.GetSection("Jwt:Key").Value;
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException("JWT Key is not configured");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("FullName", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Audience").Value
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}