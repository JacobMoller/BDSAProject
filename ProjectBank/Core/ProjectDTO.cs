namespace ProjectBank.Core;

public record ProjectDTO(int Id, string Title, Status Status, int UserId);

public record ProjectDetailsDTO(int Id, string Title, Status Status, int UserId, string? Description, DateTime CreationDate, DateTime UpdatedDate, IReadOnlyCollection<string> Tags, IReadOnlyCollection<UserDTO> Participants) : ProjectDTO(Id, Title, Status, UserId);

public record CreateProjectDTO
{
    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int UserId { get; set; }

    public Role UserRole { get; set; }

    public ICollection<string>? Tags { get; set; }
}

public record UpdateProjectDTO : CreateProjectDTO
{
    public int Id { get; set; }
}