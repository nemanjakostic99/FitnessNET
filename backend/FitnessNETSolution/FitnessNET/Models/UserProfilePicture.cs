using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FitnessNET.Models
{
    public class UserProfilePicture
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }
        public byte[] PictureData { get; set; }
        public string ContentType { get; set; } 
    }
}
