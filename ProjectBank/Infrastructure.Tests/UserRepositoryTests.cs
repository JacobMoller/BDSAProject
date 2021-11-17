namespace ProjectBank.Infrastructure.Tests;
public class UserRepositoryTests : TestSetup, IDisposable
{
    [Fact]
    public void testExample()
    {
        var testUser = new User("Jeppe", "korg@itu.dk", "test");
        _context.Add(testUser);
        _context.SaveChanges();

        var expected = "Jeppe";
        var actual = _context.Users.Find(1).Name;

        Assert.Equal(expected, actual);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}