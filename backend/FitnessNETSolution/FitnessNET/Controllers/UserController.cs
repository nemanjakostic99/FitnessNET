using FitnessNET.Data;
using FitnessNET.Models.Auth;
using FitnessNET.Models.DTO;
using FitnessNET.Services;
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
        private readonly MongoDbService _mongoDbService;
            
        public UserController(UserService userService, IConfiguration configuration, MongoDbService mongoDbService)
        {
            _userService = userService;
            _configuration = configuration;
            _mongoDbService = mongoDbService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // Extract the username from the authenticated user's claims
            var username = User.FindFirst("name")?.Value; // todo User.Identity.Name 

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

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var username = User.FindFirst("name")?.Value; // todo User.Identity.Name 

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            UserProfileDTO userProfileDTO = await this._userService.GetUserProfileAsync(username);

            if (userProfileDTO != null)
            {
                return Ok(userProfileDTO);
            }
            return NotFound("User not found or somethig went wrong");

        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDTO userProfileDTO)
        {
            var username = User.FindFirst("name")?.Value; // todo User.Identity.Name 

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            bool result = await this._userService.UpdateUserProfileAsync(username, userProfileDTO);

            if (userProfileDTO != null)
            {
                return Ok(userProfileDTO);
            }
            return NotFound("User not found or somethig went wrong");
        }

        [HttpPost("{username}/profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(string username, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var pictureData = memoryStream.ToArray();

            await _mongoDbService.UploadProfilePictureAsync(username, pictureData, file.ContentType);

            return Ok("Profile picture uploaded successfully.");
        }

        [HttpGet("{username}/profile-picture")]
        public async Task<IActionResult> GetProfilePicture(string username)
        {
            var picture = await _mongoDbService.GetProfilePictureAsync(username);

            if (picture == null)
            {
                return NotFound("Profile picture not found.");
            }

            return File(picture.PictureData, picture.ContentType);
        }
    }
}
