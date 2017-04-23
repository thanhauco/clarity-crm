using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;
using BC = BCrypt.Net.BCrypt;

namespace Clarity.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return new AuthResult { Success = false, Error = "Invalid username or password" };
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                return new AuthResult { Success = false, Error = "Invalid username or password" };
            }

            if (!user.IsActive)
            {
                return new AuthResult { Success = false, Error = "User account is inactive" };
            }

            var token = GenerateJwtToken(user);

            return new AuthResult
            {
                Success = true,
                Token = token,
                User = user
            };
        }

        public async Task<AuthResult> RegisterAsync(User user, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                return new AuthResult { Success = false, Error = "Username already exists" };
            }

            existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return new AuthResult { Success = false, Error = "Email already registered" };
            }

            user.PasswordHash = HashPassword(password);
            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;

            var createdUser = await _userRepository.CreateAsync(user);
            var token = GenerateJwtToken(createdUser);

            return new AuthResult
            {
                Success = true,
                Token = token,
                User = createdUser
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!VerifyPassword(oldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<User> GetCurrentUserAsync(string token)
        {
            var principal = ValidateAndGetPrincipal(token);
            if (principal == null) return null;

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return await _userRepository.GetByIdAsync(userId);
            }

            return null;
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            var principal = ValidateAndGetPrincipal(token);
            return Task.FromResult(principal != null);
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal ValidateAndGetPrincipal(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        private string HashPassword(string password)
        {
            return BC.HashPassword(password, BC.GenerateSalt(12));
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BC.Verify(password, hash);
        }
    }
}
