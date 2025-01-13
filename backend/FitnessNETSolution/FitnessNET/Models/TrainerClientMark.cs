using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessNET.Models
{
    public class TrainerClientMark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Required]
        public required ClientProfile Client { get; set; }

        [Required]
        public required ClientProfile Trainer { get; set; }

        [Required]
        public required Boolean Anonymous { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }
    }
}
