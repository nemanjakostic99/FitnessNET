using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessNET.Models
{
    public enum Gender
    {
        Male, Female
    }

    public class ClientProfile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string? Description { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string PasswordHash { get; set; }

        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Height must be greater than 0.")]
        public float Height { get; set; }

        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public float Weight { get; set; }

        [Required]
        public required Boolean IsTrainer {  get; set; }

        public float? TrainerAverageMark { get; set; }

        public DateTime DateRegistered { get; set; }
        public DateTime LastActive { get; set; }
    }
}
