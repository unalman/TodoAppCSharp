using Microsoft.EntityFrameworkCore;
using Todo_App.Models;

namespace Todo_App.DB
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options) { }
        public DbSet<TodoDTO>? todo { get; set; }
    }
}
