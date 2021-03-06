namespace ProjectBank.Core;
public record ProjectDTO(int Id, string Title, string Status, string SupervisorId, string? Description, DateTime CreationDate, DateTime UpdatedDate, IReadOnlyCollection<string> Tags, IReadOnlyCollection<UserDTO> Participants);

public record CreateProjectDTO
{
    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
    public string? SupervisorId { get; set; }
    public ICollection<string>? Tags { get; set; }
}

public record UpdateProjectDTO : CreateProjectDTO
{
    public int Id { get; set; }
}