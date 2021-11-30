namespace ProjectBank.Infrastructure.Tests;

public class ProjectRepositoryTests : ContextSetup, IDisposable

{
    [Fact]
    public async Task CreateProject_given_CreateProjectDTO_returns_ProjectDTO()
    {
        //Needs to return a ProjectDetailedDTO
        var project = new CreateProjectDTO
        {
            Title = "Algorithm",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "DMAT", "Fun" }
        };

        var created = await _projectRepository.CreateProjectAsync(project);

        Assert.Equal(1, created.Id);
        Assert.Equal("Algorithm", created.Title);
        Assert.Equal(Status.Active, created.Status);
        Assert.Equal(1, created.UserId);
        Assert.Equal("Sorting", created.Description);
        Assert.Equal(DateTime.UtcNow, created.CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, created.UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "DMAT", "Fun" }, created.Tags);
        Assert.Null(created.Participants);
    }

    [Fact]
    public async Task DeleteProjectById_given_ProjectId_deletes_projects()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1 });

        await _projectRepository.DeleteProjectByIdAsync(1);

        Assert.Null(_context.Projects.Find(1));
    }

    [Fact]

    public async Task EditProject_given_CreateProjectDTO_updates_project()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Algo",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "DMAT", "Fun" }
        });

        await _projectRepository.EditProjectAsync(new UpdateProjectDTO
        {
            Id = 1,
            Title = "Something else than Algo",
            Description = "Sorting",
            UserId = 1,
            Tags = new List<string> { "Economy", "Math" }
        });

        var editedProject = await _context.Projects.FindAsync(1);
        var tagNamesInProject = new List<string>();
        foreach (var tag in editedProject.Tags)
        {
            tagNamesInProject.Add(tag.Name);
        }

        Assert.Equal(1, editedProject.Id);
        Assert.Equal("Something else than Algo", editedProject.Title);
        Assert.Equal(Status.Active, editedProject.Status);
        Assert.Equal(1, editedProject.UserId);
        Assert.Equal(new List<string>() { "Economy", "Math" }, tagNamesInProject);
    }

    [Fact]

    public async Task ReadAll_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1, Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", UserId = 2, Description = "Very very fun", Tags = new List<string> { "Math" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", UserId = 3, Description = "Very very very fun", Tags = new List<string> { "Servers" } });

        var projects = await _projectRepository.ReadAllAsync();

        //project 1
        Assert.Equal(1, projects.ElementAt(0).Id);
        Assert.Equal("Algo", projects.ElementAt(0).Title);
        Assert.Equal(Status.Active, projects.ElementAt(0).Status);
        Assert.Equal(1, projects.ElementAt(0).UserId);
        Assert.Equal("Very fun", projects.ElementAt(0).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, projects.ElementAt(0).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(0).Participants);

        //project 2
        Assert.Equal(1, projects.ElementAt(1).Id);
        Assert.Equal("DMAT", projects.ElementAt(1).Title);
        Assert.Equal(Status.Active, projects.ElementAt(1).Status);
        Assert.Equal(2, projects.ElementAt(1).UserId);
        Assert.Equal("Very very fun", projects.ElementAt(1).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Math" }, projects.ElementAt(1).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(1).Participants);

        //project 2
        Assert.Equal(1, projects.ElementAt(2).Id);
        Assert.Equal("Disys", projects.ElementAt(2).Title);
        Assert.Equal(Status.Active, projects.ElementAt(2).Status);
        Assert.Equal(3, projects.ElementAt(2).UserId);
        Assert.Equal("Very very very fun", projects.ElementAt(2).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(2).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(2).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Servers" }, projects.ElementAt(2).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(2).Participants);


    }

    [Fact]
    public async Task ReadProjectById_given_ProjectId_returns_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1, Description = "Very fun", Tags = new List<string> { "Sorting" } });

        var expected = new ProjectDTO(1, "Algo", Status.Active, 1, "Very fun",);
        var actual = await _projectRepository.ReadProjectByIdAsync(1);

        Assert.Equal(expected, actual);
        Assert.Null(await _projectRepository.ReadProjectByIdAsync(100));
    }

    [Fact]
    public async Task ReadProjectsByTag_given_TagId_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1, Tags = new List<string>() { "Economy" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", UserId = 1, Tags = new List<string>() { "Economy" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", UserId = 1, Tags = new List<string>() { "Economy" } });

        var projects = await _projectRepository.ReadProjectsByTagIdAsync(1);

        Assert.Collection(projects,
        project => Assert.Equal(new ProjectDTO(1, "Algo", Status.Active, 1), project),
        project => Assert.Equal(new ProjectDTO(2, "DMAT", Status.Active, 1), project),
        project => Assert.Equal(new ProjectDTO(3, "Disys", Status.Active, 1), project)
        );
    }

    [Fact]
    public async Task ReadProjectsByUserId_given_UserId_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1 });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", UserId = 2 });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", UserId = 2 });

        var projects = await _projectRepository.ReadProjectsByUserIdAsync(2);

        Assert.Collection(projects,
        project => Assert.Equal(new ProjectDTO(2, "DMAT", Status.Active, 2), project),
        project => Assert.Equal(new ProjectDTO(3, "Disys", Status.Active, 2), project)
        );
    }

    [Fact]
    public async Task UpdateProjectStatusById_given_ProjectId_changes_status()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1 });

        await _projectRepository.CloseProjectByIdAsync(1);

        var expected = Status.Closed;
        var actual = _context.Projects.Find(1).Status;

        Assert.Equal(expected, actual);

    }

    [Fact]
    public async Task AddUserToProject_given_ProjectId_and_UserId_adds_user()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", UserId = 1 });
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Student
        });
        await _projectRepository.AddUserToProjectAsync(1, 1);
        await _projectRepository.AddUserToProjectAsync(2, 1);

        var project = await _context.Projects.FindAsync(1);

        var actual = project.Participants.ElementAt(0);
        var expected = await _context.Users.FindAsync(1);

        Assert.Equal(expected, actual);
        Assert.Throws<System.ArgumentOutOfRangeException>(() => project.Participants.ElementAt(1));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

