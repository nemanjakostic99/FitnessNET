using FitnessNET.Models;
using MongoDB.Driver;

namespace FitnessNET.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<UserProfilePicture> _profilePicturesCollection;

        public MongoDbService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
            _profilePicturesCollection = database.GetCollection<UserProfilePicture>(
                configuration["MongoDB:ProfilePicturesCollection"]
            );
        }

        public async Task UploadProfilePictureAsync(string username, byte[] pictureData, string contentType)
        {
            var existingPicture = await _profilePicturesCollection
                .Find(p => p.Username == username)
                .FirstOrDefaultAsync();

            if (existingPicture != null)
            {
                // Update existing picture
                existingPicture.PictureData = pictureData;
                existingPicture.ContentType = contentType;

                await _profilePicturesCollection.ReplaceOneAsync(
                    p => p.Username == username,
                    existingPicture
                );
            }
            else
            {
                // Insert new picture
                var newPicture = new UserProfilePicture
                {
                    Username = username,
                    PictureData = pictureData,
                    ContentType = contentType
                };

                await _profilePicturesCollection.InsertOneAsync(newPicture);
            }
        }

        public async Task<UserProfilePicture?> GetProfilePictureAsync(string username)
        {
            return await _profilePicturesCollection
                .Find(p => p.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteProfilePictureAsync(string username)
        {
            await _profilePicturesCollection.DeleteOneAsync(p => p.Username == username);
        }
    }
}
