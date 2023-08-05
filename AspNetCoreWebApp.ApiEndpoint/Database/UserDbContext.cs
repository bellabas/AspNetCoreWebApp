using AspNetCoreWebApp.ApiEndpoint.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.ApiEndpoint.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("User");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users").HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "admin", Password = "admin" });
        }
    }
}
