using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models.Auth
{
    public class ClientRegisterRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Height must be greater than 0.")]
        public float Height { get; set; }

        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public float Weight { get; set; }

        [Required]
        public required Boolean IsTrainer { get; set; }
    }
}
