namespace ProjectBank.Infrastructure.Tests;
public class UserRepositoryTests : ContextSetup, IDisposable
{
    [Fact]
    public void testExample()
    {
        var testUser = new User("Alice", "email@email.com", "password123");
        _context.Add(testUser);
        _context.SaveChanges();

        var expected = "Alice";
        var actual = _context.Users.Find(1).Name;

        Assert.Equal(expected, actual);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}