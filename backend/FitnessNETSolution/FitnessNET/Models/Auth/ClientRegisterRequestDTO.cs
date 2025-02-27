﻿using System.ComponentModel.DataAnnotations;

namespace FitnessNET.Models.Auth
{
    public class ClientRegisterRequestDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(100, ErrorMessage = "Surname length can't be more than 100 characters.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username length can't be more than 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200, ErrorMessage = "Email length can't be more than 200 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [StringLength(100, ErrorMessage = "Password length can't be more than 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Height is required.")]
        [Range(0.1, float.MaxValue, ErrorMessage = "Height must be greater than 0.")]
        public float Height { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.1, float.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public float Weight { get; set; }

        [Required(ErrorMessage = "Trainer status is required.")]
        public bool IsTrainer { get; set; }
    }
}
