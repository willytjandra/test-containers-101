using System.Net;
using System.Net.Http.Json;
using Api.Domain;
using Api.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests;

public class GetEndpointSpecification
{
    [Fact]
    public async Task Should_return_200_OK_When_user_exist()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = User.Create("John", "Doe", "john.doe@email.com");
        dbContext.Add(user);

        await dbContext.SaveChangesAsync();

        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/users/{user.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_return_a_user_When_user_exist()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = User.Create("John", "Doe", "john.doe@email.com");
        dbContext.Add(user);

        await dbContext.SaveChangesAsync();

        using var client = factory.CreateClient();

        // Act
        var result = await client.GetFromJsonAsync<User>($"/users/{user.Id}");


        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Should_return_404_NotFound_When_user_does_not_exist()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();
        using var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/users/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
