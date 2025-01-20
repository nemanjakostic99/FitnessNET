using FitnessNET.Models;
using FitnessNET.Models.DTO;

namespace FitnessNET.Services
{
    public interface IUserService
    {
        Task<UserInfoDTO> GetUserInfoAsync(string username);
        Task<UserProfileDTO> GetUserProfileAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, UserProfileDTO updatedProfile);
        Task UploadProfilePictureAsync(string username, byte[] pictureData, string contentType);
        Task<UserProfilePicture?> GetProfilePictureAsync(string username);
        Task<bool?> DeleteProfilePictureAsync(string username);
        Task<PaginatedResult<UserInfoDTO>> SearchUsersAsync(string searchTerm, string excludeUsername, bool? isTrainer = null, int page = 1, int pageSize = 10);
    }
}