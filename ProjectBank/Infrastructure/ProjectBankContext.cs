

namespace ProjectBank.Infrastructure;

public class ProjectBankContext : DbContext, IProjectBankContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Tag> Tags => Set<Tag>();

    public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Project>()
            .Property(p => p.Status)
            .HasConversion(new EnumToStringConverter<Status>());

        modelBuilder
            .Entity<User>()
            .Property(u => u.Role)
            .HasConversion(new EnumToStringConverter<Role>());

        modelBuilder.Entity<Project>()
                    .HasIndex(p => p.Title)
                    .IsUnique();
    }
}