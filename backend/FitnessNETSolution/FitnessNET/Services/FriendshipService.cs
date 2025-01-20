using FitnessNET.Data;
using FitnessNET.Models;
using FitnessNET.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FitnessNET.Services
{
    public enum FriendshipActionResult
    {
        Success,
        UserNotFound,
        FriendNotFound,
        SenderNotFound,
        ReceiverNotFound,
        RequestAlreadyExists,
        NotFriends,
        RequestNotFound,
        UnableToCreateRequest,
        GeneralFailure
    }

    public static class FriendshipActionResultExtensions
    {
        private static readonly Dictionary<FriendshipActionResult, string> Messages = new()
    {
        { FriendshipActionResult.Success, "Operation completed successfully." },
        { FriendshipActionResult.UserNotFound, "The specified user was not found." },
        { FriendshipActionResult.FriendNotFound, "The specified friend was not found." },
        { FriendshipActionResult.SenderNotFound, "The sender of the request was not found." },
        { FriendshipActionResult.ReceiverNotFound, "The receiver of the request was not found." },
        { FriendshipActionResult.RequestAlreadyExists, "A friend request already exists between the users." },
        { FriendshipActionResult.NotFriends, "The users are not friends." },
        { FriendshipActionResult.RequestNotFound, "The friend request could not be found." },
        { FriendshipActionResult.UnableToCreateRequest, "Unable to create the friend request." },
        { FriendshipActionResult.GeneralFailure, "An error occurred while processing the request." }
    };

        public static string GetMessage(this FriendshipActionResult result)
        {
            return Messages[result];
        }
    }

    public class FriendshipService
    {
        private readonly FitnessNetContext _dbContext;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly MongoDbService _mongoDbService;

        public FriendshipService(FitnessNetContext dbContext, UserService userService, IConfiguration configuration, ILogger<UserService> logger, MongoDbService mongoDbService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
            _mongoDbService = mongoDbService;
    }

        public async Task<PaginatedResult<UserInfoDTO>> SearchFriendsAsync(
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
                    .Where(u => u.Friends.Contains(user))
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
                    bool isConnected = user.Friends.Contains(u);
                    users.Add(new UserInfoDTO
                    {
                        Username = u.Username,
                        Name = u.Name,
                        Surname = u.Surname,
                        Gender = u.Gender.ToString(),
                        IsConnected = isConnected,
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

        public async Task<PaginatedResult<FriendRequstDTO>> SearchRequestsAsync(
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
                var query = _dbContext.FriendRequests
                    .Where(u => u.Receiver == user)
                    .Where(u => searchTerm == "" || // If search term is empty, return all users
                        (u.Receiver.Name.ToLower() + " " + u.Receiver.Surname.ToLower()).Contains(searchTerm) || // Search in full name
                        (u.Receiver.Surname.ToLower() + " " + u.Receiver.Name.ToLower()).Contains(searchTerm)) // Search in reversed full name
                    .Include(u => u.Sender) 
                    .Include(u => u.Receiver);

                // Get total count for pagination
                var totalItems = await query.CountAsync();

                // Calculate pagination values
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                page = Math.Max(1, Math.Min(page, totalPages)); // Ensure page is within valid range

                // Get paginated results
                var requestsList = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var requests = new List<FriendRequstDTO>();
                foreach (var u in requestsList)
                {
                    var profilePicture = await _mongoDbService.GetProfilePictureAsync(u.Sender.Username);
                    requests.Add(new FriendRequstDTO
                    {
                        Username = u.Sender.Username,
                        Name = u.Sender.Name,
                        Surname = u.Sender.Surname,
                        ProfilePicture = profilePicture == null ? null : new ProfilePictureDTO
                        {
                            PictureData = profilePicture.PictureData,
                            ContentType = profilePicture.ContentType
                        }
                    });
                }

                return new PaginatedResult<FriendRequstDTO>
                {
                    Items = requests,
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

        internal async Task<FriendshipActionResult> CreateNewFriendRequestAsync(string username, string receiverUsername)
        {
            var sender = await _userService.getUserByUsernameAsync(username);
            if (sender == null) return FriendshipActionResult.SenderNotFound;

            var receiver = await _userService.getUserByUsernameAsync(receiverUsername);
            if (receiver == null) return FriendshipActionResult.ReceiverNotFound;

            if (await FriendRequestAlreadyExist(sender, receiver) || await AreUsersFriends(sender, receiver))
                return FriendshipActionResult.RequestAlreadyExists;

            var newRequest = new FriendRequest
            {
                Sender = sender,
                Receiver = receiver
            };

            _dbContext.FriendRequests.Add(newRequest);
            await _dbContext.SaveChangesAsync();

            return FriendshipActionResult.Success;
        }

        internal async Task<FriendshipActionResult> AcceptFriendRequestAsync(string username, string senderUsername)
        {
            var receiver = await _userService.getUserByUsernameAsync(username);
            if (receiver == null) return FriendshipActionResult.ReceiverNotFound;

            var sender = await _userService.getUserByUsernameAsync(senderUsername);
            if (sender == null) return FriendshipActionResult.SenderNotFound;

            var request = await _dbContext.FriendRequests
                .FirstOrDefaultAsync(fr => fr.Sender == sender && fr.Receiver == receiver);

            if (request == null) return FriendshipActionResult.RequestNotFound;

            receiver.Friends.Add(sender);
            sender.Friends.Add(receiver);

            _dbContext.FriendRequests.Remove(request);

            // Deleting counter request
            request = await _dbContext.FriendRequests
                .FirstOrDefaultAsync(fr => fr.Sender == receiver && fr.Receiver == sender);
            if (request != null)
                _dbContext.FriendRequests.Remove(request);

            await _dbContext.SaveChangesAsync();

            return FriendshipActionResult.Success;
        }

        internal async Task<FriendshipActionResult> DeleteFriendRequestAsync(string username, string receiverUsername)
        {
            var sender = await _userService.getUserByUsernameAsync(username);
            if (sender == null) return FriendshipActionResult.SenderNotFound;

            var receiver = await _userService.getUserByUsernameAsync(receiverUsername);
            if (receiver == null) return FriendshipActionResult.ReceiverNotFound;

            var request = await _dbContext.FriendRequests
                .FirstOrDefaultAsync(fr => fr.Sender == sender && fr.Receiver == receiver);

            if (request == null) return FriendshipActionResult.RequestNotFound;

            _dbContext.FriendRequests.Remove(request);
            await _dbContext.SaveChangesAsync();

            return FriendshipActionResult.Success;
        }

        internal async Task<FriendshipActionResult> RemoveFriendAsync(string username, string friendUsername)
        {
            var user = await _userService.getUserByUsernameAsync(username);
            if (user == null) return FriendshipActionResult.UserNotFound;

            var friend = await _userService.getUserByUsernameAsync(friendUsername);
            if (friend == null) return FriendshipActionResult.FriendNotFound;

            if (!user.Friends.Contains(friend) || !friend.Friends.Contains(user))
                return FriendshipActionResult.NotFriends;

            user.Friends.Remove(friend);
            friend.Friends.Remove(user);

            await _dbContext.SaveChangesAsync();

            return FriendshipActionResult.Success;
        }

        private async Task<bool> FriendRequestAlreadyExist(ClientProfile sender, ClientProfile receiver)
        {
            FriendRequest request = await this._dbContext.FriendRequests.Where(fr => fr.Sender == sender && fr.Receiver == receiver).FirstOrDefaultAsync();
            if(request == null)
                return false;
            else return true;
        }

        private async Task<bool> AreUsersFriends(ClientProfile sender, ClientProfile receiver)
        {
            if (sender.Friends.Contains(receiver))
                return true;
            else return false;
        }

    }
}
