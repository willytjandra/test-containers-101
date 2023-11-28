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

app.MapGet("/hello", () => "Hello, world!")
    .WithName("Hello")
    .WithOpenApi();

app.MapPost("/users", PostAsync);

app.Run();

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