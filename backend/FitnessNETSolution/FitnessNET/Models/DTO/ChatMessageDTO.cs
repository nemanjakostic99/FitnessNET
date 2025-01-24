using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models.DTO
{
    public class ChatMessageDTO
    {
        public required String SenderUsername { get; set; }

        public required String ReceiverUsername { get; set; }

        public required string Content { get; set; }

        public DateTime SentAt { get; set; } 
    }
}
