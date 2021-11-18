namespace ProjectBank.Core;

public record ProjectDTO(int Id, string Title, string? Description, Status Status, int UserId);

public record ProjectDetailsDTO(int Id, string Title, string? Description, Status Status, int UserId, DateTime CreationDate, DateTime UpdatedDate, IReadOnlyCollection<Tag> Tags, IReadOnlyCollection<User> Participants): ProjectDTO(Id, Title, Description, Status, UserId);

public record CreateProjectDTO
{
    [StringLength(50)]
    public string Title { get; init; }

    [StringLength(500)]
    public string? Description { get; init; }

    public Status Status { get; init; }

    public int UserId { get; init; }

    [DataType(DataType.Date)]
    public DateTime CreationDate { get; init; }

    [DataType(DataType.Date)]
    public DateTime? UpdatedDate { get; init; }
    public ICollection<Tag>? Tags { get; init; }

    public ICollection<User>? Participants { get; init; }

}

public record UpdateProjectDTO : CreateProjectDTO
{
    public int Id { get; init; }
}


