using FitnessNET.Models.DTO;
using FitnessNET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessNET.Controllers
{
    [Route("api/messageHistory")]
    [ApiController]
    public class MessageHistoryController : Controller
    {
        private readonly MessageService _messageService;
        private readonly IConfiguration _configuration;

        public MessageHistoryController(
            MessageService messageService,
            IConfiguration configuration)
        {
            _messageService = messageService;
            _configuration = configuration;
        }

        [HttpGet("searchMessageHistory")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<ChatMessageDTO>>> SearchMessageHistory(
            [FromQuery] string user2 = "",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var user1 = User.FindFirst("name")?.Value;
                if (user1 == null)
                {
                    return Unauthorized();
                }

                var result = await _messageService.GetPaginatedChatHistoryAsync(
                    user1,
                    user2,
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
    }
}
