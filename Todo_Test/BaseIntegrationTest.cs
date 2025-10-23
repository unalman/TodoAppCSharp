using Microsoft.Extensions.DependencyInjection;
using Todo_App.DB;

namespace Todo_Test
{
    public class BaseIntegrationTest : IClassFixture<IntegrationTestFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly TodoContext _todoContext;

        protected BaseIntegrationTest(IntegrationTestFactory factory)
        {
            _scope = factory.Services.CreateScope();
            _todoContext = _scope.ServiceProvider.GetRequiredService<TodoContext>();
        }
    }
}
