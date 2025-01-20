using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models
{
    public class FriendRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Required]
        public required ClientProfile Sender { get; set; }

        [Required]
        public required ClientProfile Receiver { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
