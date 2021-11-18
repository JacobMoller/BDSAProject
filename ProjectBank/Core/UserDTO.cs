namespace ProjectBank.Core;

public record UserDTO(int Id, string? Name, string? Email, string? Password);

public record CreateUserDTO
{
    [StringLength(50)]
    public string? Name { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    [StringLength(50)]
    public string? Password { get; init; }

    public Role Role { get; init; }
}

public record UpdateUserDTO : CreateUserDTO
{
    public int Id { get; init; }

    public ICollection<ProjectDTO>? Projects { get; init; }
}
