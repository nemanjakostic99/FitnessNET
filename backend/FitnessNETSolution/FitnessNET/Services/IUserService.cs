using FitnessNET.Models;
using FitnessNET.Models.DTO;

namespace FitnessNET.Services
{
    public interface IUserService
    {
        Task<UserInfoDTO> GetUserInfoAsync(string username);
        Task<byte[]> GetUserProfilePictureAsync(int userId);
        Task<UserProfileDTO> GetUserProfileAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, UserProfileDTO updatedProfile);
    }
}