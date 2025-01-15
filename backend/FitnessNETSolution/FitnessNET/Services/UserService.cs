using FitnessNET.Data;
using FitnessNET.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;

public class UserService : IUserService
{
    private readonly FitnessNetContext _dbContext;

    public UserService(FitnessNetContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserInfoDTO> GetUserInfoAsync(string username)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return new UserInfoDTO
        {

            Name = user.Name,
            Surname = user.Surname,
            Username = user.Username
        };
    }

    public async Task<byte[]> GetUserProfilePictureAsync(int userId)
    {
        //var user = await _context.ClientProfiles
        //    .FirstOrDefaultAsync(u => u.ID == userId);

        //if (user == null)
        //{
        //    throw new KeyNotFoundException("User not found.");
        //}

        //// Assuming profile picture is stored as a byte array in the database
        //// Adjust logic if stored elsewhere (e.g., file system, cloud storage)
        //if (user.Description == null)
        //{
        //    throw new Exception("No profile picture found.");
        //}

        //return Convert.FromBase64String(user.Description); // Example of a base64-encoded image
        return null;
    }
}
