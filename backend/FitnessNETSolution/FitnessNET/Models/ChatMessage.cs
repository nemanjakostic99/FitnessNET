using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models
{
    public class ChatMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Required]
        public required ClientProfile Sender { get; set; }

        [Required]
        public required ClientProfile Receiver { get; set; }

        [Required]
        [MaxLength(400, ErrorMessage = "Message cannot exceed 400 characters.")]
        public required string Content { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
