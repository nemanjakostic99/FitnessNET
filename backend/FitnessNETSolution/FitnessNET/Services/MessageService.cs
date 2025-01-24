using FitnessNET.Data;
using FitnessNET.Models;
using FitnessNET.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace FitnessNET.Services
{
    public class MessageService
    {
        private readonly FitnessNetContext _dbContext;
        private readonly UserService _userService;
        private readonly ILogger<UserService> _logger;

        public MessageService(
            FitnessNetContext dbContext,
            UserService userService,
            ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _userService = userService; 
            _logger = logger;
        }

        public async Task SaveMessageAsync(String senderUsername, String receiverUsername, String text)
        {
            var receiver = await _userService.getUserByUsernameAsync(receiverUsername);
            if (receiver == null) return; 

            var sender = await _userService.getUserByUsernameAsync(senderUsername);
            if (sender == null) return;

            ChatMessage message = new ChatMessage() { 
                Receiver = receiver,
                Sender = sender,
                Content = text };
            

            _dbContext.ChatMessages.Add(message);
            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task<PaginatedResult<ChatMessageDTO>> GetPaginatedChatHistoryAsync(string user1, string user2, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var query = _dbContext.ChatMessages
                .Where(m =>
                    (m.Sender.Username == user1 && m.Receiver.Username == user2) ||
                    (m.Sender.Username == user2 && m.Receiver.Username == user1))
                .OrderByDescending(m => m.Timestamp); 

            
            var totalMessages = await query.CountAsync();

            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new ChatMessageDTO
                {
                    SenderUsername = m.Sender.Username,
                    ReceiverUsername = m.Receiver.Username,
                    Content = m.Content,
                    SentAt = m.Timestamp
                })
                .ToListAsync();


            return new PaginatedResult<ChatMessageDTO>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalMessages,
                Items = messages
            };
        }
    }
}

