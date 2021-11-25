namespace ProjectBank.Infrastructure.Tests;

public class ProjectRepositoryTests : ContextSetup, IDisposable

{
    [Fact]
    public async Task CreateProject_given_CreateProjectDTO_returns_ProjectDTO()
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

    public void DeleteProjectById_given_Id_deletes_projects()
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

    public async void EditProject_given_Algo_updates_to_something_else()
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

    [Fact]

    public async void ReadAll_returns_list_of_projects()
    {
        //Arrange
        _context.Projects.AddRange(
        new Project("Algo", Status.Active, 1),
        new Project("DMAT", Status.Active, 2),
        new Project("Disys", Status.Active, 3)
        );
        _context.SaveChanges();

        var projects = await _repository.ReadAllAsync();

        //Assert
        Assert.Collection(projects,
        project => Assert.Equal(new ProjectDTO(1, "Algo", Status.Active, 1), project),
        project => Assert.Equal(new ProjectDTO(2, "DMAT", Status.Active, 2), project),
        project => Assert.Equal(new ProjectDTO(3, "Disys", Status.Active, 3), project)
        );
    }

    [Fact]
    public async void ReadProjectById_given_Id_returns_ProjectDTO()
    {
        //Arrange
        _context.Projects.Add(new Project("Algo", Status.Active, 1));
        _context.SaveChanges();

        var expected = new ProjectDTO(1, "Algo", Status.Active, 1);
        var actual = await _repository.ReadProjectByIdAsync(1);

        //Assert
        Assert.Equal(expected, actual);
        Assert.Null(await _repository.ReadProjectByIdAsync(100));
    }

    [Fact]
    public async void ReadProjectsByTag_given_TagId_returns_list_of_ProjectDTO()
    {
        //Arrange
        _context.Projects.Add(new Project("Algo", Status.Active, 1)
        {
            Tags = new List<Tag>() { new Tag("Economy") }
        });
        _context.SaveChanges();

        var expected = new List<ProjectDTO>() { new ProjectDTO(1, "Algo", Status.Active, 1) };

        var actual = await _repository.ReadProjectsByTagIdAsync(1);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void ReadProjectsByUserId_given_UserId_returns_list_of_ProjectDTO()
    {
        //Arrange
        _context.Projects.AddRange(
        new Project("Algo", Status.Active, 1),
        new Project("DMAT", Status.Active, 2),
        new Project("Disys", Status.Active, 2)
        );
        _context.SaveChanges();

        var projects = await _repository.ReadProjectsByUserIdAsync(2);

        //Assert
        Assert.Collection(projects,
        project => Assert.Equal(new ProjectDTO(2, "DMAT", Status.Active, 2), project),
        project => Assert.Equal(new ProjectDTO(3, "Disys", Status.Active, 2), project)
        );
    }

    [Fact]
    public async Task UpdateProjectStatusById_given_ProjectId_changes_status()
    {
        //Arrange
        _context.Projects.Add(new Project("DMAT", Status.Active, 1));
        await _context.SaveChangesAsync();

        await _repository.UpdateProjectStatusByIdAsync(1);

        var expected = Status.Closed;
        var actual = await _context.Projects.FindAsync(1);
        //Assert
        Assert.Equal(expected, actual.Status);

    }

    public void Dispose()
    {
        _context.Dispose();
    }
}