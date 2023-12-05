using Api.Domain;
using Api.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class UsersEndpoints
{
    public static WebApplication UseUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapPost("", CreateUserAsync)
            .WithName("CreateUser")
            .WithOpenApi();

        group.MapGet("{id:guid}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithOpenApi();

        group.MapDelete("{id:guid}", DeleteUserAsync)
            .WithName("DeleteUser")
            .WithOpenApi();

        return app;
    }

    static async Task<Results<Ok<User>, NotFound>> GetUserByIdAsync(
        Guid id,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync(id, cancellationToken);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(user);
    }

    static async Task<Results<Created<User>, ProblemHttpResult>> CreateUserAsync(
        UserDto userDto,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var user = User.Create(userDto.FirstName, userDto.LastName, userDto.Email);

        dbContext.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"users/{user.Id}", user);
    }

    static async Task<NoContent> DeleteUserAsync(
        Guid id,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    record UserDto(string FirstName, string LastName, string Email);
}
