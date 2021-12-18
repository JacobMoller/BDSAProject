namespace Server.Integration.Tests;

public class ProjectTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    public ProjectTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Get_returns_projects()
    {
        var projects = await _client.GetFromJsonAsync<ProjectDTO[]>("/api/projects");

        Assert.NotNull(projects);
        Assert.True(projects.Length >= 3);
        Assert.Contains(projects, p => p.Title == "Super Fun Project");
    }

    [Fact]
    public async Task Get_returns_specific_project()
    {
        var project = await _client.GetFromJsonAsync<ProjectDTO>("/api/projects/1");

        Assert.NotNull(project);
        Assert.Equal(1, project.Id);
        Assert.Equal("Super Fun Project", project.Title);
    }

    [Fact]
    public async Task Post_returns_Created()
    {
        var project = new CreateProjectDTO
        {
            Title = "Nice project title",
            Description = "A short project description",
            SupervisorId = "1",
            Tags = new List<string>() { "DMAT", "Algo" },
        };


        var response = await _client.PostAsJsonAsync("/api/projects", project);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.Equal(new Uri("http://localhost/api/Projects/4"), response.Headers.Location);

        var created = await response.Content.ReadFromJsonAsync<ProjectDTO>();

        Assert.NotNull(created);
        Assert.Equal(4, created.Id);
        Assert.Equal("Nice project title", created.Title);
    }

    [Fact]
    public async Task Put_returns_NoContent()
    {
        var updatedProject = new UpdateProjectDTO() { Id = 2, Title = "Test" };
        var response = await _client.PutAsJsonAsync("/api/projects/2", updatedProject);
        var project = await _client.GetFromJsonAsync<ProjectDTO>("/api/projects/2");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("Test", project.Title);
    }

    public async Task Delete_returns_Status_NoContent()
    {
        var response = await _client.DeleteAsync("api/projects/1");
        var project = await _client.GetFromJsonAsync<ProjectDTO>("/api/projects/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Null(project);
    }
}

