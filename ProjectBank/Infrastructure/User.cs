namespace ProjectBank.Infrastructure;

public class User
{
    [Key]
    public string Id { get; init; }

    [StringLength(50)]
    public string Name { get; init; }

    public Role Role { get; init; }

    public ICollection<Project>? Projects { get; init; }

    public User(string Id, string Name, Role Role)
    {
        this.Id = Id;
        this.Name = Name;
        this.Role = Role;
    }

}

