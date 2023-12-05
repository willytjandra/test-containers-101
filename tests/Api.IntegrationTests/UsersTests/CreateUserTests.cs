using System.Net;
using System.Net.Http.Json;
using Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.IntegrationTests.UsersTests;

public class CreateUserTests : BaseIntegrationTest
{
    public CreateUserTests(IntegrationTestApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_return_201_Created_When_send_create_user_request()
    {
        // Arrange
        var newUserRequest = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"/users", newUserRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Should_contain_location_header_When_send_create_user_request()
    {
        // Arrange
        var newUserRequest = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"/users", newUserRequest);

        // Assert
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Should_contain_location_header_to_created_resource_When_send_create_user_request()
    {
        // Arrange
        var newUserRequest = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"/users", newUserRequest);

        var createdResource = await response.Content.ReadFromJsonAsync<User>();

        // Assert
        Assert.Equal($"users/{createdResource!.Id}", response.Headers.Location!.ToString());
    }

    [Fact]
    public async Task Should_add_user_to_database_When_send_create_user_request()
    {
        // Arrange
        var newUserRequest = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"/users", new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        });

        // Assert
        var dbEntry = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == "john.doe@mail.com");
        Assert.NotNull(dbEntry);
    }
}
