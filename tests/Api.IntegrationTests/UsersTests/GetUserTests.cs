using System.Net;
using System.Net.Http.Json;
using Api.Domain;

namespace Api.IntegrationTests.UsersTests;

public class GetUserTests : BaseIntegrationTest
{
    public GetUserTests(IntegrationTestApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_return_200_OK_When_user_exist()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john.doe@email.com");
        DbContext.Add(user);

        await DbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"/users/{user.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_return_a_user_When_user_exist()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john.doe@email.com");
        DbContext.Add(user);

        await DbContext.SaveChangesAsync();

        // Act
        var result = await Client.GetFromJsonAsync<User>($"/users/{user.Id}");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Should_return_404_NotFound_When_user_does_not_exist()
    {
        // Arrange

        // Act
        var response = await Client.GetAsync($"/users/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
