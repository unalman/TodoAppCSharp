using Microsoft.EntityFrameworkCore;
using Todo_App.DB;
using Todo_App.Models;

namespace Todo_Test
{
    public class TodoContextTests
    {
        /// <summary>
        /// UseInMemoryDatabase  test sýrasýnda RAM’de bir DB yaratýr, gerçek PostgreSQL gerekmez.
        /// </summary>
        /// <returns></returns>
        private DbContextOptions<TodoContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }
        [Fact]
        public void CanAddTodo()
        {
            var options = GetInMemoryOptions();

            using (var context = new TodoContext(options))
            {
                var todo = new TodoDTO() { id = 1, text = "Test Todo" };
                context.todo.Add(todo);
                context.SaveChanges();
            }
            using (var context = new TodoContext(options))
            {
                var todos = context.todo.ToList();
                Assert.Single(todos);
                Assert.Equal("Test Todo", todos[0].text);
                context.Database.EnsureDeleted();
                context.Dispose();
            }
          
        }
        [Fact]
        public void CanRemoveTodo()
        {
            var options = GetInMemoryOptions();

            using (var context = new TodoContext(options))
            {
                var todo = new TodoDTO() { id = 1, text = "Test Todo" };
                context.todo.Add(todo);
                context.SaveChanges();

                context.todo.Remove(todo);
                context.SaveChanges();
            }
            using (var context = new TodoContext(options))
            {
                var todos = context.todo.ToList();
                Assert.Empty(todos);
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}
