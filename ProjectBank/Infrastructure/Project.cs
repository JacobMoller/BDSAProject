namespace ProjectBank.Infrastructure;

public class Project
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Title is too long")]
    public string Title { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Description is too long")]
    public string? Description { get; set; }

    public Status Status { get; set; }

    public string UserId { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreationDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime UpdatedDate { get; set; }
    
    public ICollection<Tag>? Tags { get; set; }

    public ICollection<User>? Participants { get; set; }


    public Project(string Title, Status Status, string UserId)
    {
        this.Title = Title;
        this.Status = Status;
        this.UserId = UserId;
    }
}

