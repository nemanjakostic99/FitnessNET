using FitnessNET.Models.DTO;
using FitnessNET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessNET.Controllers
{
    [Route("api/friendship")]
    [ApiController]
    public class FriendshipController : Controller
    {

        private readonly FriendshipService _friendshipService;
        private readonly IConfiguration _configuration;

        public FriendshipController(FriendshipService friendshipService, IConfiguration configuration)
        {
            _friendshipService = friendshipService;
            _configuration = configuration;
        }

        [HttpGet("searchFriends")]
        public async Task<ActionResult<PaginatedResult<UserInfoDTO>>> SearchFriends(
            [FromQuery] string searchTerm = "",
            [FromQuery] bool? isTrainer = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var currentUsername = User.FindFirst("name")?.Value;
                if (currentUsername == null)
                {
                    return Unauthorized();
                }

                var result = await _friendshipService.SearchFriendsAsync(
                    searchTerm,
                    currentUsername,
                    isTrainer,
                    page,
                    pageSize);

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

        [HttpGet("searchRequests")]
        public async Task<ActionResult<PaginatedResult<UserInfoDTO>>> SearchRequests(
            [FromQuery] string searchTerm = "",
            [FromQuery] bool? isTrainer = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var currentUsername = User.FindFirst("name")?.Value;
                if (currentUsername == null)
                {
                    return Unauthorized();
                }

                var result = await _friendshipService.SearchRequestsAsync(
                    searchTerm,
                    currentUsername,
                    isTrainer,
                    page,
                    pageSize);

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

        [HttpPost("send-request")]
        [Authorize]
        public async Task<IActionResult> SendRequest([FromQuery] String receiverUsername)
        {
            var username = User.FindFirst("name")?.Value; 

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            var message = await this._friendshipService.CreateNewFriendRequestAsync(username, receiverUsername);

            if(message == FriendshipActionResult.Success)
            {
                return Ok(FriendshipActionResultExtensions.GetMessage(message));
            }
            else return BadRequest(FriendshipActionResultExtensions.GetMessage(message));
        }

        [HttpPost("accept-request")]
        [Authorize]
        public async Task<IActionResult> AccpetRequest([FromQuery] String senderUsername)
        {
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            var message = await this._friendshipService.AcceptFriendRequestAsync(senderUsername, username);

            if (message == FriendshipActionResult.Success)
            {
                return Ok(FriendshipActionResultExtensions.GetMessage(message));
            }
            else return BadRequest(FriendshipActionResultExtensions.GetMessage(message));
        }

        [HttpDelete("remove-request")]
        [Authorize]
        public async Task<IActionResult> RemoveRequest([FromQuery] String senderUsername)
        {
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            var message = await this._friendshipService.DeleteFriendRequestAsync(senderUsername, username);

            if (message == FriendshipActionResult.Success)
            {
                return Ok(FriendshipActionResultExtensions.GetMessage(message));
            }
            else return BadRequest(FriendshipActionResultExtensions.GetMessage(message));
        }

        [HttpDelete("remove-friend")]
        [Authorize]
        public async Task<IActionResult> RemoveFriend([FromQuery] String friend)
        {
            var username = User.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Username not found in token.");
            }

            var message = await this._friendshipService.RemoveFriendAsync(username, friend);

            if (message == FriendshipActionResult.Success)
            {
                return Ok(FriendshipActionResultExtensions.GetMessage(message));
            }
            else return BadRequest(FriendshipActionResultExtensions.GetMessage(message));
        }
    }
}
