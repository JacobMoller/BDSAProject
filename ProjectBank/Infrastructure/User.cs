namespace ProjectBank.Infrastructure;

public class User
{
    [Key]
    public int Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }

    public Role Role { get; init; }

    public ICollection<Project>? Projects { get; init; }

    public User(string Name, string Email, string Password, Role Role)
    {
        this.Name = Name;
        this.Email = Email ;
        this.Password = Password;
        this.Role = Role;
    }

}

