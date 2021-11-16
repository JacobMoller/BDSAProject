namespace ProjectBank.Infrastructure;

public class User
{
    [Key]
    public int Id { get; init; }

    [StringLength(50)]
    public string Name { get; init; }

    [EmailAddress]
    public string Email { get; init; }

    [StringLength(50)]
    public string Password { get; init; }

    public Role Role { get; init; }

    public ICollection<Project>? Projects { get; init; }

    public User(string Name, string Email, string Password)
    {
        this.Name = Name;
        this.Email = Email;
        this.Password = Password;
    }

}

