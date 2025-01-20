using FitnessNET.Data;
using FitnessNET.Models;
using FitnessNET.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;

namespace FitnessNET.Services;

public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}

public class UserService : IUserService
{
    private readonly FitnessNetContext _dbContext;
    private readonly MongoDbService _mongoDbService;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        FitnessNetContext dbContext, 
        MongoDbService mongoDbService,
        IImageProcessingService imageProcessingService,
        ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _mongoDbService = mongoDbService;
        _imageProcessingService = imageProcessingService;
        _logger = logger;
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

    public async Task UploadProfilePictureAsync(string username, byte[] pictureData, string contentType)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        try
        {
            // Convert byte array to stream for image processing
            using var imageStream = new MemoryStream(pictureData);
            
            // Process the image to 800x800
            byte[] processedImageData = await _imageProcessingService.ProcessProfileImageAsync(imageStream);
            
            // Save the processed image to MongoDB
            await _mongoDbService.UploadProfilePictureAsync(
                user.Username, 
                processedImageData, 
                "image/jpeg" // ImageProcessingService always outputs JPEG
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process and upload profile picture for user {Username}", username);
            throw new ApplicationException("Failed to process and upload profile picture", ex);
        }
    }

    public async Task<UserProfilePicture?> GetProfilePictureAsync(string username)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return await this._mongoDbService.GetProfilePictureAsync(user.Username);
    }

    public async Task<bool?> DeleteProfilePictureAsync(string username)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return await this._mongoDbService.DeleteProfilePictureAsync(user.Username);
    }

    public async Task<PaginatedResult<UserInfoDTO>> SearchUsersAsync(
        string searchTerm, 
        string excludeUsername,
        bool? isTrainer = null,
        int page = 1, 
        int pageSize = 10)
    {
        var user = await _dbContext.ClientProfiles.FirstOrDefaultAsync(u => u.Username == excludeUsername);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        try
        {
            // Normalize search term
            searchTerm = searchTerm?.Trim().ToLower() ?? "";

            // Create base query
            var query = _dbContext.ClientProfiles
                .Where(u => u.Username != excludeUsername) // Exclude the requesting user
                .Where(u => !isTrainer.HasValue || u.IsTrainer == isTrainer) // Filter by trainer status if specified
                .Where(u => searchTerm == "" || // If search term is empty, return all users
                    (u.Name.ToLower() + " " + u.Surname.ToLower()).Contains(searchTerm) || // Search in full name
                    (u.Surname.ToLower() + " " + u.Name.ToLower()).Contains(searchTerm)); // Search in reversed full name

            // Get total count for pagination
            var totalItems = await query.CountAsync();

            // Calculate pagination values
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages)); // Ensure page is within valid range

            // Get paginated results
            var usersList = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var users = new List<UserInfoDTO>();
            foreach (var u in usersList)
            {
                var profilePicture = await _mongoDbService.GetProfilePictureAsync(u.Username);
                users.Add(new UserInfoDTO
                {
                    Username = u.Username,
                    Name = u.Name,
                    Surname = u.Surname,
                    Gender = u.Gender.ToString(),
                    ProfilePicture = profilePicture == null ? null : new ProfilePictureDTO 
                    { 
                        PictureData = profilePicture.PictureData,
                        ContentType = profilePicture.ContentType 
                    }
                });
            }

            return new PaginatedResult<UserInfoDTO>
            {
                Items = users,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw new ApplicationException("Failed to search users", ex);
        }
    }
}
