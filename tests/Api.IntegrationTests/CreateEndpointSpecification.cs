using System.Net;
using System.Net.Http.Json;
using Api.Domain;
using Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests;

public class CreateEndpointSpecification
{
    [Fact]
    public async Task Should_return_201_Created_When_send_create_user_request()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();

        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync($"/users", new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        });

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Should_contain_location_header_When_send_create_user_request()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();

        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync($"/users", new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        });

        // Assert
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Should_contain_location_header_to_created_resource_When_send_create_user_request()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();

        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync($"/users", new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        });

        var createdResource = await response.Content.ReadFromJsonAsync<User>();

        // Assert
        Assert.Equal($"users/{createdResource!.Id}", response.Headers.Location!.ToString());
    }

    [Fact]
    public async Task Should_add_user_to_database_When_send_create_user_request()
    {
        // Arrange
        var factory = new IntegrationTestApiFactory();

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        using var client = factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync($"/users", new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@mail.com"
        });

        // Assert
        var list = await dbContext.Users.ToListAsync();
        var dbEntry = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "john.doe@mail.com");
        Assert.NotNull(dbEntry);
    }
}
