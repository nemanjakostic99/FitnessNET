using FitnessNET.Data;
using FitnessNET.Models;
using FitnessNET.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FitnessNET.Services;
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

    public async Task<UserProfileDTO> GetUserProfileAsync(string username)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return new UserProfileDTO
        { 
            Name = user.Name,
            Surname = user.Surname,
            Username = user.Username,
            Gender = user.Gender,
            Description = user.Description, 
            Email = user.Email, 
            Height = user.Height,   
            Weight = user.Weight    
        };
    }

    public async Task<bool> UpdateUserProfileAsync(string username, UserProfileDTO updatedProfile)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.Name = updatedProfile.Name;
        user.Surname = updatedProfile.Surname;
        user.Email = updatedProfile.Email;
        user.Gender = (Gender)updatedProfile.Gender; 
        user.Height = updatedProfile.Height;
        user.Weight = updatedProfile.Weight;
        user.Description = updatedProfile.Description;  

        _ = _dbContext.SaveChangesAsync();

        return true;
    }
}
