namespace FitnessNET.Models.DTO
{
    public class FriendRequstDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public bool IsTrainer { get; set; } = false;
        public ProfilePictureDTO? ProfilePicture { get; set; }
    }
}
