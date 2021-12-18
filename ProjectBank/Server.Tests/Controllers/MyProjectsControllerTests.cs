namespace Server.Tests;

public class MyProjectsControllerTests
{
    Mock<IProjectRepository> repository = new Mock<IProjectRepository>();

    [Fact]
    public async Task Get_returns_MyProjects_from_repo()
    {
        // Arrange
        var project0 = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        var project1 = new ProjectDTO(2, "Very very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        var expected = new List<ProjectDTO>() { project0, project1 };
        repository.Setup(m => m.ReadProjectsBySupervisorIdAsync("1")).ReturnsAsync(expected);
        var controller = new MyProjectsController(repository.Object) { GetObjectId = () => "1" };

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }
}