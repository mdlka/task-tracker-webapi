using Microsoft.EntityFrameworkCore;

namespace TaskTrackerWebAPI.Entities
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) 
            : base(options)
        { }
        
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Board> Boards { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCredentials> UsersCredentials { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}