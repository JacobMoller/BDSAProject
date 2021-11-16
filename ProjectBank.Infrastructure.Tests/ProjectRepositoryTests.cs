namespace ProjectBank.Infrastructure.Tests;
public class ProjectRepositoryTests : TestSetup, IDisposable
{
    [Fact]
    public void testExample()
    {
        var testProject = new Project("Test", Status.Active, 1);
        _context.Add(testProject);
        _context.SaveChanges();

        var expected = "Test";
        var actual = _context.Projects.Find(1).Title;

        Assert.Equal(expected, actual);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}



