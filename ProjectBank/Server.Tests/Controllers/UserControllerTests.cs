namespace Server.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task Post_creates_User()
    {
        // Arrange
        var toCreate = new CreateUserDTO();
        var created = new UserDTO("1", "Alice");

        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.CreateUserAsync(toCreate)).ReturnsAsync(created);
        var controller = new UserController(repository.Object) { GetObjectId = () => "1" };

        // Act
        var result = await controller.Post(toCreate) as CreatedAtActionResult;

        // Assert
        Assert.Equal(created, result?.Value);
        Assert.Equal("Get", result?.ActionName);
    }

    [Fact]
    public async Task Get_given_existing_returns_project()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        var user = new UserDetailsDTO("1", "Alice", "Supervisor");
        repository.Setup(m => m.ReadUserByIdAsync("1")).ReturnsAsync(user);
        var controller = new UserController(repository.Object);

        // Act
        var response = await controller.Get("1");

        // Assert
        Assert.Equal(user, response.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.ReadUserByIdAsync("1")).ReturnsAsync(default(UserDetailsDTO));
        var controller = new UserController(repository.Object);

        // Act
        var response = await controller.Get("42");

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }
}