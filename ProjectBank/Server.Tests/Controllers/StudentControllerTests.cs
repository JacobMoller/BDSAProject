namespace Server.Tests;

public class StudentControllerTests
{
    Mock<IProjectRepository> repository = new Mock<IProjectRepository>();

    [Fact]
    public async Task Put_given_valid_id_returns_NoContent()
    {
        var project0 = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        repository.Setup(m => m.AddUserToProjectAsync("1", 1)).ReturnsAsync(Response.Updated);
        var controller = new StudentController(repository.Object) { GetObjectId = () => "1" };

        var response = await controller.Put(1, project0);

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_UserId_returns_NotFound()
    {
        var project0 = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        repository.Setup(m => m.AddUserToProjectAsync("hejhej", 1)).ReturnsAsync(Response.NotFound);
        var controller = new StudentController(repository.Object) { GetObjectId = () => "hejhej" };

        var response = await controller.Put(1, project0);

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_projectId_returns_NotFound()
    {
        var project0 = new ProjectDTO(42, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>(){new UserDTO("1", "Charlie")});
        repository.Setup(m => m.AddUserToProjectAsync("1", 42)).ReturnsAsync(Response.NotFound);
        var controller = new StudentController(repository.Object) { GetObjectId = () => "1" };

        var response = await controller.Put(42, project0);

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Get_returns_YourProjects_from_repo()
    {
        // Arrange
        var project0 = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>(){new UserDTO("1", "Charlie")});
        var expected = new List<ProjectDTO>() {project0};
        repository.Setup(m => m.ReadProjectsByStudentIdAsync("1")).ReturnsAsync(expected);
        var controller = new StudentController(repository.Object){ GetObjectId = () => "1" };

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_not_applied_returns_zero_projects()
    {
        // Arrange
        var expected = new List<ProjectDTO>();
        repository.Setup(m => m.ReadProjectsByStudentIdAsync("1")).ReturnsAsync(expected);
        var controller = new StudentController(repository.Object){ GetObjectId = () => "1" };

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }
}