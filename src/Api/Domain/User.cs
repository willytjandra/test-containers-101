using System.Text.Json.Serialization;

namespace Api.Domain;

public class User
{
    [JsonConstructor]
    private User() { }

    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public string FirstName { get; private set; } = string.Empty;

    [JsonInclude]
    public string LastName { get; private set; } = string.Empty;

    [JsonInclude]
    public string Email { get; private set; } = string.Empty;

    public static User Create(string firstname, string lastname, string email) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = firstname,
        LastName = lastname,
        Email = email
    };
}
