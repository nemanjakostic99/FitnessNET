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
            var username = User.FindFirst("name")?.Value; 

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
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDTO userProfileDTO)
        {
            var username = User.FindFirst("name")?.Value; 

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool result = await this._userService.UpdateUserProfileAsync(username, userProfileDTO);

            if (userProfileDTO != null)
            {
                return Ok(userProfileDTO);
            }
            return NotFound("User not found or somethig went wrong");
        }

        [HttpPut("profile-picture")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            if (profilePicture == null || profilePicture.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            using var memoryStream = new MemoryStream();    
            await profilePicture.CopyToAsync(memoryStream);
            var pictureData = memoryStream.ToArray();

            await _userService.UploadProfilePictureAsync(username, pictureData, profilePicture.ContentType);

            return Ok("Profile picture uploaded successfully.");
        }

        [HttpGet("profile-picture")]
        [Authorize]
        public async Task<IActionResult> GetProfilePicture()
        {
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            var picture = await _userService.GetProfilePictureAsync(username);

            if (picture == null)
            {
                return NotFound("Profile picture not found.");
            }

            return File(picture.PictureData, picture.ContentType);
        }

        [HttpDelete("profile-picture")]
        [Authorize]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            var result = await _userService.DeleteProfilePictureAsync(username);

            if (result == null)
            {
                return NotFound("Profile picture not found.");
            }

            return Ok("Picture deleted");
        }

        [HttpGet("searchUsers")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<UserInfoDTO>>> SearchUsers(
            [FromQuery] string searchTerm = "",
            [FromQuery] bool? isTrainer = null,
            [FromQuery] int page = 1)
        {
            try
            {
                var currentUsername = User.FindFirst("name")?.Value;
                if (currentUsername == null)
                {
                    return Unauthorized();
                }

                var result = await _userService.SearchUsersAsync(
                    searchTerm,
                    currentUsername,
                    isTrainer,
                    page,
                    10);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
