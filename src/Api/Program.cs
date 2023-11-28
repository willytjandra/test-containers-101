using Api.Domain;
using Api.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    optionsBuilder.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users/{id:guid}", GetAsync)
    .WithName("GetUser")
    .WithOpenApi();

app.MapPost("/users", PostAsync)
    .WithName("CreateUser")
    .WithOpenApi();

app.Run();

static async Task<Results<Ok<User>, NotFound>> GetAsync(
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

static async Task<Results<Ok<User>, ProblemHttpResult>> PostAsync(
    UserDto userDto,
    ApplicationDbContext dbContext,
    CancellationToken cancellationToken)
{
    var user = User.Create(userDto.FirstName, userDto.LastName, userDto.Email);

    dbContext.Add(user);

    await dbContext.SaveChangesAsync(cancellationToken);

    return TypedResults.Ok(user);
}

record UserDto(string FirstName, string LastName, string Email);