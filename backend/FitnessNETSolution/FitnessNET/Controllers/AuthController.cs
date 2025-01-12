using FitnessNET.Data;
using FitnessNET.Models;
using FitnessNET.Models.Auth;
using FitnessNET.Utils;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FitnessNET.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FitnessNetContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(FitnessNetContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClientRegisterRequest request)
        {
            // Check if the email or username already exists
            if (_dbContext.ClientProfiles.Any(u => u.Email == request.Email || u.Username == request.Username))
            {
                return BadRequest("Email or Username already exists.");
            }

            // Create a new user with a hashed password
            var newUser = new ClientProfile
            {
                Name = request.Name,
                Surname = request.Surname,
                Username = request.Username,
                IsTrainer = request.IsTrainer,  
                Email = request.Email,
                Height = request.Height,
                Weight = request.Weight,
                PasswordHash = PasswordHelper.HashPassword(request.Password)
            };

            _dbContext.ClientProfiles.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ClientLoginRequest request)
        {
            // Find the user by email
             var user = _dbContext.ClientProfiles.SingleOrDefault(u => u.Username == request.Username);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT token
            var token = JwtHelper.GenerateToken(
                user,
                _configuration["Jwt:Key"],
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"]
            );

            return Ok(new { Token = token });
        }
    }
}
