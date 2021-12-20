namespace Server.Tests;

public class StudentControllerTests
{
    Mock<IProjectRepository> repository = new Mock<IProjectRepository>();

    [Fact]
    public async Task Put_given_valid_id_returns_NoContent()
    {

        repository.Setup(m => m.AddUserToProjectAsync("1", 1)).ReturnsAsync(Response.Updated);
        var controller = new StudentController(repository.Object) { GetObjectId = () => "1" };

        var response = await controller.Put(1);

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_UserId_returns_NotFound()
    {
        repository.Setup(m => m.AddUserToProjectAsync("hejhej", 1)).ReturnsAsync(Response.NotFound);
        var controller = new StudentController(repository.Object) { GetObjectId = () => "hejhej" };

        var response = await controller.Put(1);

        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Put_given_unknown_projectId_returns_NotFound()
    {
        repository.Setup(m => m.AddUserToProjectAsync("1", 42)).ReturnsAsync(Response.NotFound);
        var controller = new StudentController(repository.Object) { GetObjectId = () => "1" };

        var response = await controller.Put(42);

        Assert.IsType<NotFoundResult>(response);
    }

}