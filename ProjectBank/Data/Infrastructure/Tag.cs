using ProjectBank.Data.Infrastructure;
public class Tag
{
    [Key]
    public int Id { get; init; }

    [StringLength(50)]
    public string Name { get; init; }

    public ICollection<Project>? Projects { get; init; }

    public Tag(string Name)
    {
        this.Name = Name;
    }
}
