using FitnessNET.Data;
using FitnessNET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FitnessNET.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string receiverUsername, string message)
        {
            var username = Context.User?.FindFirst("name")?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                await this._messageService.SaveMessageAsync(username, receiverUsername, message);
                await Clients.Group(receiverUsername).SendAsync("ReceiveMessage", username, message);
            }
        }

        
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.FindFirst("name")?.Value;  
            if (!string.IsNullOrEmpty(username))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, username);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User?.FindFirst("name")?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
