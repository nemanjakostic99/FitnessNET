using FitnessNET.Data;
using FitnessNET.Models.Auth;
using FitnessNET.Models.DTO;
using FitnessNET.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessNET.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // Extract the username from the authenticated user's claims
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            UserInfoDTO userDTO = await this._userService.GetUserInfoAsync(username);

            if (userDTO != null)
            {
                return Ok(userDTO); 
            }
            return NotFound("User not found or somethig went wrong");

        }
    }
}
