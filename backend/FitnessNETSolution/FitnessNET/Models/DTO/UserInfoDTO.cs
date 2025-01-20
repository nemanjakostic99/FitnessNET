namespace FitnessNET.Models.DTO
{
    public class ProfilePictureDTO
    {
        public byte[]? PictureData { get; set; }
        public string? ContentType { get; set; }
    }

    public class UserInfoDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public ProfilePictureDTO? ProfilePicture { get; set; }
    }
}
