namespace ProjectBank.Infrastructure.Tests;
public abstract class ContextSetup
{
    protected readonly ProjectBankContext _context;
    protected readonly ProjectRepository _projectRepository;
    protected readonly UserRepository _userRepository;
    public ContextSetup()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        _context = context;
        _projectRepository = new ProjectRepository(_context);
        _userRepository = new UserRepository(_context);
    }
}