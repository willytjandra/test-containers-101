using Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<IntegrationTestApiFactory>, IDisposable
{
    public BaseIntegrationTest(IntegrationTestApiFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Client = factory.CreateClient();
        DbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    private readonly IServiceScope _scope;
    protected HttpClient Client { get; }
    protected ApplicationDbContext DbContext { get; }

    public void Dispose()
    {
        _scope.Dispose();
        Client.Dispose();
        DbContext.Dispose();
    }
}
