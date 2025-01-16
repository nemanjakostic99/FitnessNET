namespace FitnessNET.Models.DTO
{
    public class UserProfileDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string? Description { get; set; }
    }
}
