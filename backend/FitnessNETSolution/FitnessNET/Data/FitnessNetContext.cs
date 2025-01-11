using FitnessNET.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessNET.Data
{
    public class FitnessNetContext : DbContext
    {
        public FitnessNetContext(DbContextOptions<FitnessNetContext> options) : base(options)
        {
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<TrainerClientMark> TrainerClientMarks { get; set; }
    }
}
