namespace ProjectBank.Infrastructure;

public class Project
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(500)]
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

