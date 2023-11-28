namespace Api.Domain;

public class User
{
    private User() { }

    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public static User Create(string firstname, string lastname, string email) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = firstname,
        LastName = lastname,
        Email = email
    };
}
