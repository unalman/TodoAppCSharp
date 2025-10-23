using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Todo_App.DB;

namespace Todo_Test
{
    public class PostgresContainerFixture : IAsyncLifetime
    {
        public PostgreSqlContainer Container { get; }

        public PostgresContainerFixture()
        {
            Container = new PostgreSqlBuilder()
                .WithDatabase("testdb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithImage("postgres:15-alpine")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();
        }

        public string ConnectionString => Container.GetConnectionString();

        public DbContextOptions<TodoContext> CreateTodoContextOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>()
                .UseNpgsql(ConnectionString);
            return optionsBuilder.Options;
        }
        public TodoContext CreateTodoContext()
        {
            var options = CreateTodoContextOptions();
            var context = new TodoContext(options);
            return context;
        }
    }
}
