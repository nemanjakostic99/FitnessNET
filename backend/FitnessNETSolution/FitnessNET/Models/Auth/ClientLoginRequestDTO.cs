using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models.Auth
{
    public class ClientLoginRequestDTO
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
