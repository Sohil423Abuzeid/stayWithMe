using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace stayWithMeApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // friends
            modelBuilder.Entity<User>()
            .HasMany(u => u.Friends)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
            "UserUsers",
            j => j
              .HasOne<User>()
              .WithMany()
              .HasForeignKey("UserId")
              .OnDelete(DeleteBehavior.Restrict),
            j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("RelatedUserId")
                .OnDelete(DeleteBehavior.Restrict),
            j =>
         {
            j.HasKey("UserId", "RelatedUserId");
            });




            // user
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.userName)
                .IsUnique();

        }
        public DbSet<User> Users { get; set; }

        public DbSet<friendRequest> FriendRequests { get; set; }

        public DbSet<Effect> Effects { get; set; }

        public DbSet<otpRequest> otpRequests { get; set; }

    }
}
