namespace Server.Tests;

public class ProjectsControllerTests
{
    Mock<IProjectRepository> repository = new Mock<IProjectRepository>();

    [Fact]
    public async Task Post_creates_Project()
    {
        // Arrange
        var toCreate = new CreateProjectDTO();
        var created = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        repository.Setup(m => m.CreateProjectAsync(toCreate)).ReturnsAsync(created);
        var controller = new ProjectsController(repository.Object) { GetObjectId = () => "1" };

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(created, result?.Value);
        Assert.Equal("Get", result?.ActionName);
        Assert.Equal(KeyValuePair.Create("Id", (object?)1), result?.RouteValues?.Single());
    }

    [Fact]
    public async Task Get_returns_Projects_from_repo()
    {
        // Arrange
        var project0 = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        var project1 = new ProjectDTO(2, "Very very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        var expected = new List<ProjectDTO>() { project0, project1 };
        repository.Setup(m => m.ReadAllAsync()).ReturnsAsync(expected);
        var controller = new ProjectsController(repository.Object);

        // Act
        var actual = await controller.Get();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Arrange
        repository.Setup(m => m.ReadProjectByIdAsync(42)).ReturnsAsync(default(ProjectDTO));
        var controller = new ProjectsController(repository.Object);

        // Act
        var response = await controller.Get(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Get_given_existing_returns_project()
    {
        // Arrange
        var project = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        repository.Setup(m => m.ReadProjectByIdAsync(1)).ReturnsAsync(project);
        var controller = new ProjectsController(repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(project, response.Value);
    }

    [Fact]
    public async Task Put_updates_Character()
    {
        // Arrange
        var project = new UpdateProjectDTO();
        repository.Setup(m => m.EditProjectAsync(1, project)).ReturnsAsync(Response.Updated);
        var controller = new ProjectsController(repository.Object);

        // Act
        var response = await controller.Put(1, project);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_id_returns_NotFound()
    {
        // Arrange
        var project = new UpdateProjectDTO();
        repository.Setup(m => m.EditProjectAsync(1, project)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectsController(repository.Object);

        // Act
        var response = await controller.Put(1, project);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_non_existing_returns_NotFound()
    {
        // Arrange
        repository.Setup(m => m.DeleteProjectByIdAsync(42)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectsController(repository.Object);

        // Act
        var response = await controller.Delete(42);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_given_existing_returns_NoContent()
    {
        // Arrange
        repository.Setup(m => m.DeleteProjectByIdAsync(1)).ReturnsAsync(Response.Deleted);
        var controller = new ProjectsController(repository.Object);

        // Act
        var response = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }
}