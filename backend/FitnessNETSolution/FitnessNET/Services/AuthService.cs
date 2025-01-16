using FitnessNET.Data;
using FitnessNET.Models;
using FitnessNET.Models.Auth;
using FitnessNET.Utils;
using Microsoft.Extensions.Configuration;

namespace FitnessNET.Services
{
    public class AuthService
    {
        private readonly FitnessNetContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(FitnessNetContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, string Token)> RegisterAsync(ClientRegisterRequestDTO request)
        {
            // Check if email or username already exists
            if (_dbContext.ClientProfiles.Any(u => u.Email == request.Email || u.Username == request.Username))
            {
                return (false, "Email or Username already exists.", null);
            }

            // Create new user
            var newUser = new ClientProfile
            {
                Name = request.Name,
                Surname = request.Surname,
                Username = request.Username,
                IsTrainer = request.IsTrainer,
                Email = request.Email,
                Height = request.Height,
                Weight = request.Weight,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                DateRegistered = DateTime.UtcNow,
                LastActive = DateTime.UtcNow  
            };

            _dbContext.ClientProfiles.Add(newUser);
            await _dbContext.SaveChangesAsync();

            // Verify user was saved correctly
            var user = _dbContext.ClientProfiles.SingleOrDefault(u => u.Username == request.Username);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return (false, "New user not saved", null);
            }

            // Generate JWT token
            var token = JwtHelper.GenerateToken(
                user,
                _configuration["Jwt:Key"],
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"]
            );

            return (true, "User registered successfully.", token);
        }

        public async Task<(bool Success, string Message, string Token)> LoginAsync(ClientLoginRequestDTO request)
        {
            // Find user by username
            var user = _dbContext.ClientProfiles.SingleOrDefault(u => u.Username == request.Username);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return (false, "Invalid username or password.", null);
            }

            // Generate JWT token
            var token = JwtHelper.GenerateToken(
                user,
                _configuration["Jwt:Key"],
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"]
            );


            user.LastActive = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return (true, "Login successful.", token);
        }
    }
}
