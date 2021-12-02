namespace ProjectBank.Core;

public record UserDTO(int Id, string? Name);
public record UserDetailsDTO(int Id, string? Name, string? Email, string? Password);
public record CreateUserDTO
{
    [StringLength(50)]
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Password { get; set; }

    public Role Role { get; set; }
}

public record UpdateUserDTO : CreateUserDTO
{
    public int Id { get; set; }


    public ICollection<ProjectDTO>? Projects { get; set; }
}
