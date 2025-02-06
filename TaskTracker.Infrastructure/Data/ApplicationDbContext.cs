using Microsoft.EntityFrameworkCore;
using TaskTracker.Core.Models;

namespace TaskTracker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        { }
        
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Board> Boards { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCredentials> UsersCredentials { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}