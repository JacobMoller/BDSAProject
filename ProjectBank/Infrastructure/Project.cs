namespace ProjectBank.Infrastructure;

public class Project
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(50, ErrorMessage = "Tihi migrations virker da :P")]
    public string Title { get; set; }

    [StringLength(500, ErrorMessage = "Migrations virker :P")]
    public string? Description { get; set; }

    public Status Status { get; set; }

    public string SupervisorId { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreationDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime UpdatedDate { get; set; }

    public ICollection<Tag>? Tags { get; set; }

    public ICollection<User>? Participants { get; set; }


    public Project(string Title, Status Status, string SupervisorId)
    {
        this.Title = Title;
        this.Status = Status;
        this.SupervisorId = SupervisorId;
    }
}

