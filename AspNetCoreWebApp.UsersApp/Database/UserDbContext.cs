using AspNetCoreWebApp.UsersApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.UsersApp.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("User");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users").HasKey(x => x.Username);
            modelBuilder.Entity<User>().HasData(new User { Username = "admin", Password = "admin" });
        }
    }
}
