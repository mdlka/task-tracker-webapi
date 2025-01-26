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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userId = Guid.NewGuid();
            const string email = "test@mail.ru";

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = userId,
                Email = email,
                Nickname = "test"
            });

            modelBuilder.Entity<UserCredentials>().HasData(new UserCredentials()
            {
                Id = userId,
                Login = email,
                Password = "123",
                Version = 1
            });
        }
    }
}