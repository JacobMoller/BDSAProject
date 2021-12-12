using Xunit;

namespace Server.Tests;

public class ProjectsControllerTests
{

    /*[Fact]
    public async Task Create_creates_Project()
    {
        // Arrange
        //var logger = new Mock<ILogger<ProjectsController>>();
        var toCreate = new CreateProjectDTO();
        //public record ProjectDTO(int Id, string Title, string Status, string SupervisorId, string? Description, DateTime CreationDate, DateTime UpdatedDate, IReadOnlyCollection<string> Tags, IReadOnlyCollection<UserDTO> Participants);
        var created = new ProjectDTO(1, "Very nice project", "Active", "1", "A created project", DateTime.UtcNow, DateTime.UtcNow, new List<string>(), new List<UserDTO>());
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.CreateProjectAsync(toCreate)).ReturnsAsync(created);
        var controller = new ProjectsController(repository.Object);
        //System.Security.Claims.ClaimsPrincipal.Id user =
        var userName = "Alice";
        var controllerContext = new Mock<ProjectsController>();
        var principal = new Moq.Mock<IPrincipal>();
        principal.Setup(p => p.IsInRole("Supervisor")).Returns(true);
        principal.SetupGet(x => x.Identity.Name).Returns(userName);
        controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
        controller.ControllerContext = controllerContext.Object;

        /*protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }*/

    // Act
    /*var result = await controller.Post(toCreate) as CreatedAtActionResult;

    // Assert
    Assert.Equal(created, result?.Value);
        Assert.Equal("Get", result?.ActionName);
        Assert.Equal(KeyValuePair.Create("Id", (object?)1), result?.RouteValues?.Single());
    }*/

    [Fact]
    public async Task Get_returns_Project_from_repo()
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

    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IProjectRepository>();
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
        var repository = new Mock<IProjectRepository>();
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
        var project = new CharacterUpdateDto();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.EditProjectAsync(1, project)).ReturnsAsync(Updated);
        var controller = new CharactersController(repository.Object);

        // Act
        var response = await controller.Put(1, character);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }
}