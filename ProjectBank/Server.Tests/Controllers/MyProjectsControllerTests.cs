namespace Server.Tests;

public class MyProjectsControllerTests
{
    [Fact]
    public async Task Get_returns_MyProjects_from_repo()
    {
        // Arrange
        var expected = Array.Empty<ProjectDTO>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadProjectsBySupervisorIdAsync("1")).ReturnsAsync(expected);
        var controller = new MyProjectsController(repository.Object) { GetObjectId = () => "1" };

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }
}