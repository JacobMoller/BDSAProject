namespace ProjectBank.Infrastructure.Tests;
public abstract class ContextSetup
{
    protected readonly ProjectBankContext _context;
    protected readonly ProjectRepository _repository;
    public ContextSetup()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        _context = context;
        _repository = new ProjectRepository();

        addTestData();
    }

    public void addTestData()
    {
        //Testdata should be added to _context.
    }
}