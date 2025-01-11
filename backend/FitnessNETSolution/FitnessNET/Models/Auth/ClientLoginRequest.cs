using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models.Auth
{
    public class ClientLoginRequest
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
