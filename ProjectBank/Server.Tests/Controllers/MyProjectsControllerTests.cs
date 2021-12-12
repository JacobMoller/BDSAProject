namespace Server.Tests;

public class MyProjectsController
{
    [Fact]
    public async Task Get_returns_MyProjects_from_repo()
    {
        // Arrange
        var expected = Array.Empty<ProjectDTO>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new ProjectsController(repository.Object);

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }
}