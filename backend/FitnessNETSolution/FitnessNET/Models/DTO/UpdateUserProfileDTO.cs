using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models.DTO
{
    public class UpdateUserProfileDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(100, ErrorMessage = "Surname length can't be more than 100 characters.")]
        public string Surname { get; set; }

        [StringLength(50, ErrorMessage = "Username length can't be more than 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200, ErrorMessage = "Email length can't be more than 200 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Range(30, 250, ErrorMessage = "Height must be between 30 and 250 cm.")]
        public float Height { get; set; }

        [Range(10, 500, ErrorMessage = "Weight must be between 10 and 500 kg.")]
        public float Weight { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string? Description { get; set; }
    }
}
