using Microsoft.EntityFrameworkCore;
using TheGramProfile.Properties.Models;

namespace TheGramProfile.Repository
{
    public class ProfileContext : DbContext
    {
        public DbSet<UserProfile> Profiles { get; set; }

        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().ToTable("Profiles");
        }
    }
}