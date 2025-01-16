namespace FitnessNET.Models.DTO
{
    public class ChangePasswordDTO
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }

        public ChangePasswordDTO(string newPassword = "", string oldPassword = "")
        {
            NewPassword = newPassword;
            OldPassword = oldPassword;
        }
    }

}
