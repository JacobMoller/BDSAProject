namespace ProjectBank.Infrastructure.Tests;

public class ProjectRepositoryTests : ContextSetup, IDisposable

{
    [Fact]
    public async Task CreateAsync_creates_new_character_with_generated_id()
    {
        //Needs to return a ProjectDetailedDTO
        //Arrange
        var project = new CreateProjectDTO
        {
            Title = "Algorithm",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "DMAT", "Fun" }
        };

        //Act
        var created = await _repository.CreateProjectAsync(project);

        //Assert
        Assert.Equal(1, created.Id);
        Assert.Equal("Algorithm", created.Title);
        Assert.Equal(Status.Active, created.Status);
        Assert.Equal(1, created.UserId);
    }

    [Fact]

    public void DeleteProject_deletes_projects_with_id()
    {

        //Arrange
        var project = new CreateProjectDTO
        {
            Title = "Algorithm",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "DMAT", "Fun" }
        };

        //Act
        var created = _repository.CreateProjectAsync(project);

        _repository.DeleteProjectByIdAsync(1);

        Assert.Null(_context.Projects.Find(1));
    }

    [Fact]

    public async void UpdateProject_projects_with_id()
    {
        //Arrange
        var project = new CreateProjectDTO
        {
            Title = "Algorithm",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "DMAT", "Fun" }
        };

        var Updated = new UpdateProjectDTO
        {
            Id = 1,
            Title = "Something else than Algo",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "DMAT", "Fun" }
        };

        //Act
        var created = await _repository.CreateProjectAsync(project);

        _repository.EditProjectAsync(Updated);

        var updatedProject = await _context.Projects.FindAsync(1);

        //Assert
        Assert.Equal(1, updatedProject.Id);
        Assert.Equal("Something else than Algo", updatedProject.Title);
        Assert.Equal(Status.Active, updatedProject.Status);
        Assert.Equal(1, updatedProject.UserId);
    }

    // [Fact]

    // public async Task ReadAllAsync()
    // {
    // //Arrange
    // var project1 = new CreateProjectDTO
    // {
    //     Title = "Algo",
    //     UserId = 1,
    // };

    // var project2 = new CreateProjectDTO
    // {
    //     Title = "DMAT",
    //     UserId = 2,
    // };
    // var project3 = new CreateProjectDTO
    // {
    //     Title = "Fun Project",
    //     UserId = 3,
    // };

    // var expected = new List<ProjectDTO>();

    // //Act
    // var created = await _repository.CreateProjectAsync(project1);
    // var created1 = await _repository.CreateProjectAsync(project2);
    // var created2 = await _repository.CreateProjectAsync(project3);

    // var projects = await _repository.ReadAllAsync();


    // Assert.Collection(projects,
    //     project => Assert.Equal(new ProjectDTO(1, "Algo", Status.Active, 1),
    //     project => Assert.Equal(new ProjectDTO(2, "DMAT", Status.Active, 2),
    //     project => Assert.Equal(new ProjectDTO(3, "Fun Project", Status.Active, 3)
    // );

    // }



    public void Dispose()
    {
        _context.Dispose();
    }
}