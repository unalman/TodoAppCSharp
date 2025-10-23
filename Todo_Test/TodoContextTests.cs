using Microsoft.EntityFrameworkCore;
using Todo_App.DB;
using Todo_App.Models;

namespace Todo_Test
{
    public class TodoContextTests : IClassFixture<PostgresContainerFixture>, IDisposable
    {
        private readonly PostgresContainerFixture _fixture;
        private readonly TodoContext _todoContext;

        public TodoContextTests(PostgresContainerFixture fixture)
        {
            _fixture = fixture;

            _todoContext = _fixture.CreateTodoContext();
            _todoContext.Database.EnsureDeleted();
            _todoContext.Database.Migrate();
        }

        [Fact]
        public void CanAddTodo()
        {
            var todo = new TodoDTO() { text = "Test Todo" };
            
            _todoContext.todo.Add(todo);
            _todoContext.SaveChanges();

            var todos = _todoContext.todo.ToList();
            Assert.Single(todos);
            Assert.Equal("Test Todo", todos[0].text);
        }
        [Fact]
        public void CanRemoveTodo()
        {
            var todo = new TodoDTO() { text = "Test Todo" };
            _todoContext.todo.Add(todo);
            _todoContext.SaveChanges();

            _todoContext.todo.Remove(todo);
            _todoContext.SaveChanges();

            var todos = _todoContext.todo.ToList();
            Assert.Empty(todos);
        }
        public void Dispose()
        {
            _todoContext?.Dispose();
        }
    }
}
