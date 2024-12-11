using Microsoft.EntityFrameworkCore;

namespace TaskTrackerWebAPI.Entities
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) 
            : base(options)
        { }
        
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoBoard> TodoBoards { get; set; }
    }
}