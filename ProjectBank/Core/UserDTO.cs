namespace ProjectBank.Core;

public record UserDTO(string Id, string? Name);
public record UserDetailsDTO(string Id, string? Name, Role? Role);
public record CreateUserDTO
{
    public string? Id{ get; set;} 
    [StringLength(50)]
    public string? Name { get; set; }

    public Role Role { get; set; }
}

public record UpdateUserDTO : CreateUserDTO
{
    public ICollection<ProjectDTO>? Projects { get; set; }
}
