namespace ProjectBank.Infrastructure.Tests;

public class UserRepositoryTests : ContextSetup, IDisposable

{

    public void Dispose()
    {
        _context.Dispose();
    }
}