using Microsoft.EntityFrameworkCore;

namespace ProjectBank.Infrastructure;
public interface IProjectBankContext : IDisposable
{
    DbSet<Project> Projects { get; }
    DbSet<User> Users { get; }
    DbSet<Tag> Tags { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}