namespace ProjectBank.Infrastructure.Tests;

public class ProjectRepositoryTests : ContextSetup, IDisposable

{
    [Fact]
    public async Task CreateProject_given_CreateProjectDTO_returns_ProjectDTO()
    {
        var project = new CreateProjectDTO
        {
            Title = "Algorithm",
            Description = "Sorting",
            SupervisorId = "1",
            Tags = new List<string> { "DMAT", "Fun" }
        };

        var created = await _projectRepository.CreateProjectAsync(project);

        Assert.Equal(1, created.Id);
        Assert.Equal("Algorithm", created.Title);
        Assert.Equal("Active", created.Status);
        Assert.Equal("1", created.SupervisorId);
        Assert.Equal("Sorting", created.Description);
        Assert.Equal(DateTime.UtcNow, created.CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, created.UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "DMAT", "Fun" }, created.Tags);
        Assert.Equal(new List<UserDTO>(), created.Participants);
    }

    [Fact]
    public async Task DeleteProjectById_given_ProjectId_deletes_projects()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });

        var actual = await _projectRepository.DeleteProjectByIdAsync(1);
        var expected = Response.Deleted;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task DeleteProjectById_given_not_existing_returns_NotFound()
    {
        Assert.Equal(Response.NotFound, await _projectRepository.DeleteProjectByIdAsync(1));
    }

    [Fact]
    public async Task EditProject_given_UpdateProjectDTO_updates_project_changes_existing_tags()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.EditProjectAsync(1, new UpdateProjectDTO { Id = 1, Title = "Something else than Algo", Description = "Not sorting", SupervisorId = "1", Tags = new List<string> { "Economy", "Math" } });
        Project? editedProject = await _context.Projects.FindAsync(1);

        Assert.NotNull(editedProject);
        Assert.Equal(1, editedProject.Id);
        Assert.Equal("Something else than Algo", editedProject.Title);
        Assert.Equal(Status.Active, editedProject.Status);
        Assert.Equal("1", editedProject.SupervisorId);
        Assert.Equal("Not sorting", editedProject.Description);
        Assert.Equal(DateTime.UtcNow, editedProject.UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.NotNull(editedProject.Tags);
        Assert.Equal(new List<string>() { "Economy", "Math" }, editedProject.Tags.Select(tag => new string(tag.Name)).ToList());

        Assert.Equal(Response.NotFound, await _projectRepository.EditProjectAsync(2, new UpdateProjectDTO() { Id = 2, Title = "Does not exist" }));
    }

    [Fact]
    public async Task EditProject_given_UpdateProjectDTO_updates_project_adds_to_existing_tags()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.EditProjectAsync(1, new UpdateProjectDTO { Id = 1, Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting", "Economy", "Math" } });
        var editedProjectWithTags = await _context.Projects.FindAsync(1);

        Assert.NotNull(editedProjectWithTags);
        Assert.Equal(1, editedProjectWithTags.Id);
        Assert.Equal("Algo", editedProjectWithTags.Title);
        Assert.Equal(Status.Active, editedProjectWithTags.Status);
        Assert.Equal("1", editedProjectWithTags.SupervisorId);
        Assert.Equal("Very fun", editedProjectWithTags.Description);
        Assert.Equal(DateTime.UtcNow, editedProjectWithTags.UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.NotNull(editedProjectWithTags.Tags);
        Assert.Equal(new List<string>() { "Sorting", "Economy", "Math" }, editedProjectWithTags.Tags.Select(tag => new string(tag.Name)).ToList());
    }

    [Fact]
    public async Task EditProject_given_UpdateProjectDTO_updates_project_removes_tags()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", SupervisorId = "2", Description = "Very very fun", Tags = new List<string> { "Math" } });
        await _projectRepository.EditProjectAsync(1, new UpdateProjectDTO { Id = 1, Title = "DMAT 2", SupervisorId = "2", Description = "Very fun" });
        var editedProjectWithoutTags = await _context.Projects.FindAsync(1);

        Assert.NotNull(editedProjectWithoutTags);
        Assert.Equal(1, editedProjectWithoutTags.Id);
        Assert.Equal("DMAT 2", editedProjectWithoutTags.Title);
        Assert.Equal(Status.Active, editedProjectWithoutTags.Status);
        Assert.Equal("2", editedProjectWithoutTags.SupervisorId);
        Assert.Equal("Very fun", editedProjectWithoutTags.Description);
        Assert.Equal(DateTime.UtcNow, editedProjectWithoutTags.UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.NotNull(editedProjectWithoutTags.Tags);
        Assert.Equal(new List<string>(), editedProjectWithoutTags.Tags.Select(tag => new string(tag.Name)).ToList());
    }

    [Fact]
    public async Task ReadAll_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", SupervisorId = "2", Description = "Very very fun", Tags = new List<string> { "Math" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", SupervisorId = "3", Description = "Very very very fun", Tags = new List<string> { "Servers" } });

        var projects = await _projectRepository.ReadAllAsync();

        //Project0
        Assert.Equal(1, projects.ElementAt(0).Id);
        Assert.Equal("Algo", projects.ElementAt(0).Title);
        Assert.Equal("Active", projects.ElementAt(0).Status);
        Assert.Equal("1", projects.ElementAt(0).SupervisorId);
        Assert.Equal("Very fun", projects.ElementAt(0).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, projects.ElementAt(0).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(0).Participants);

        //Project1
        Assert.Equal(2, projects.ElementAt(1).Id);
        Assert.Equal("DMAT", projects.ElementAt(1).Title);
        Assert.Equal("Active", projects.ElementAt(1).Status);
        Assert.Equal("2", projects.ElementAt(1).SupervisorId);
        Assert.Equal("Very very fun", projects.ElementAt(1).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Math" }, projects.ElementAt(1).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(1).Participants);

        //Project2
        Assert.Equal(3, projects.ElementAt(2).Id);
        Assert.Equal("Disys", projects.ElementAt(2).Title);
        Assert.Equal("Active", projects.ElementAt(2).Status);
        Assert.Equal("3", projects.ElementAt(2).SupervisorId);
        Assert.Equal("Very very very fun", projects.ElementAt(2).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(2).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(2).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Servers" }, projects.ElementAt(2).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(2).Participants);
    }

    [Fact]
    public async Task ReadProjectById_given_ProjectId_returns_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "1", Name = "Alice", Role = Role.Student });

        var actual = await _projectRepository.ReadProjectByIdAsync(1);

        Assert.Equal(1, actual.Value.Id);
        Assert.Equal("Algo", actual.Value.Title);
        Assert.Equal("Active", actual.Value.Status);
        Assert.Equal("1", actual.Value.SupervisorId);
        Assert.Equal("Very fun", actual.Value.Description);
        Assert.Equal(DateTime.UtcNow, actual.Value.CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, actual.Value.UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, actual.Value.Tags);
        Assert.Equal(new List<UserDTO>(), actual.Value.Participants);

        var notExistingProject = await _projectRepository.ReadProjectByIdAsync(100);
        Assert.True(notExistingProject.IsNone);

        await _projectRepository.AddUserToProjectAsync("1", 1);
        Assert.Equal(new UserDTO("1", "Alice"), _projectRepository.ReadProjectByIdAsync(1).Result.Value.Participants.ElementAt(0));
    }

    [Fact]
    public async Task ReadProjectsByTagId_given_TagId_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", SupervisorId = "2", Description = "Very very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", SupervisorId = "3", Description = "Very very very fun", Tags = new List<string> { "Servers" } });

        var projects = await _projectRepository.ReadProjectsByTagIdAsync(1);

        //Project0
        Assert.Equal(1, projects.ElementAt(0).Id);
        Assert.Equal("Algo", projects.ElementAt(0).Title);
        Assert.Equal("Active", projects.ElementAt(0).Status);
        Assert.Equal("1", projects.ElementAt(0).SupervisorId);
        Assert.Equal("Very fun", projects.ElementAt(0).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, projects.ElementAt(0).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(0).Participants);

        //Project1
        Assert.Equal(2, projects.ElementAt(1).Id);
        Assert.Equal("DMAT", projects.ElementAt(1).Title);
        Assert.Equal("Active", projects.ElementAt(1).Status);
        Assert.Equal("2", projects.ElementAt(1).SupervisorId);
        Assert.Equal("Very very fun", projects.ElementAt(1).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, projects.ElementAt(1).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(1).Participants);


        Assert.Throws<System.ArgumentOutOfRangeException>(() => projects.ElementAt(3));
    }

    [Fact]
    public async Task ReadProjectsBySupervisorId_given_SupervisorId_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", SupervisorId = "1", Description = "Very very fun", Tags = new List<string> { "Math" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", SupervisorId = "2", Description = "Very very very fun", Tags = new List<string> { "Servers" } });

        var projects = await _projectRepository.ReadProjectsBySupervisorIdAsync("1");

        //Project0
        Assert.Equal(1, projects.ElementAt(0).Id);
        Assert.Equal("Algo", projects.ElementAt(0).Title);
        Assert.Equal("Active", projects.ElementAt(0).Status);
        Assert.Equal("1", projects.ElementAt(0).SupervisorId);
        Assert.Equal("Very fun", projects.ElementAt(0).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, projects.ElementAt(0).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(0).Participants);

        //Project1
        Assert.Equal(2, projects.ElementAt(1).Id);
        Assert.Equal("DMAT", projects.ElementAt(1).Title);
        Assert.Equal("Active", projects.ElementAt(1).Status);
        Assert.Equal("1", projects.ElementAt(1).SupervisorId);
        Assert.Equal("Very very fun", projects.ElementAt(1).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Math" }, projects.ElementAt(1).Tags);
        Assert.Equal(new List<UserDTO>(), projects.ElementAt(1).Participants);

        Assert.Throws<System.ArgumentOutOfRangeException>(() => projects.ElementAt(3));
    }

    [Fact]
    public async Task ReadProjectsByStudentId_given_StudentId_returns_list_of_ProjectDTO()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "DMAT", SupervisorId = "1", Description = "Very very fun", Tags = new List<string> { "Math" } });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Disys", SupervisorId = "2", Description = "Very very very fun", Tags = new List<string> { "Servers" } });
        await _userRepository.CreateUserAsync(new CreateUserDTO() { Id = "1", Name = "Alice", Role = Role.Student });
        await _projectRepository.AddUserToProjectAsync("1", 1);
        await _projectRepository.AddUserToProjectAsync("1", 2);
        await _projectRepository.AddUserToProjectAsync("1", 3);


        var projects = await _projectRepository.ReadProjectsByStudentIdAsync("1");

        //Project0
        Assert.Equal(1, projects.ElementAt(0).Id);
        Assert.Equal("Algo", projects.ElementAt(0).Title);
        Assert.Equal("Active", projects.ElementAt(0).Status);
        Assert.Equal("1", projects.ElementAt(0).SupervisorId);
        Assert.Equal("Very fun", projects.ElementAt(0).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(0).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Sorting" }, projects.ElementAt(0).Tags);
        Assert.Equal(new List<UserDTO>() { new UserDTO("1", "Alice") }, projects.ElementAt(0).Participants);

        //Project1
        Assert.Equal(2, projects.ElementAt(1).Id);
        Assert.Equal("DMAT", projects.ElementAt(1).Title);
        Assert.Equal("Active", projects.ElementAt(1).Status);
        Assert.Equal("1", projects.ElementAt(1).SupervisorId);
        Assert.Equal("Very very fun", projects.ElementAt(1).Description);
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).CreationDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(DateTime.UtcNow, projects.ElementAt(1).UpdatedDate, precision: TimeSpan.FromSeconds(5));
        Assert.Equal(new List<string>() { "Math" }, projects.ElementAt(1).Tags);
        Assert.Equal(new List<UserDTO>() { new UserDTO("1", "Alice") }, projects.ElementAt(1).Participants);

        Assert.Throws<System.ArgumentOutOfRangeException>(() => projects.ElementAt(3));
    }

    [Fact]
    public async Task AddUserToProject_given_ProjectId_and_StudentId_adds_user()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "1", Name = "Alice", Role = Role.Student });
        await _projectRepository.AddUserToProjectAsync("1", 1);

        var project = await _context.Projects.FindAsync(1);
        Assert.NotNull(project);
        var actual = project.Participants != null ? project.Participants.ElementAt(0) : null;
        var expected = await _context.Users.FindAsync("1");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AddUserToProject_given_not_exitsting_studentID_returns_NotFound()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        Assert.Equal(Response.NotFound, await _projectRepository.AddUserToProjectAsync("2", 1));
    }


    [Fact]
    public async Task AddUserToProject_with_5_users_changes_status_to_closed()
    {
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO { Title = "Algo", SupervisorId = "1", Description = "Very fun", Tags = new List<string> { "Sorting" } });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "1", Name = "Alice", Role = Role.Student });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "2", Name = "Bob", Role = Role.Student });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "3", Name = "Charlie", Role = Role.Student });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "4", Name = "Dave", Role = Role.Student });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "5", Name = "Emma", Role = Role.Student });
        await _userRepository.CreateUserAsync(new CreateUserDTO { Id = "6", Name = "Felicia", Role = Role.Student });
        await _projectRepository.AddUserToProjectAsync("1", 1);
        await _projectRepository.AddUserToProjectAsync("2", 1);
        await _projectRepository.AddUserToProjectAsync("3", 1);
        await _projectRepository.AddUserToProjectAsync("4", 1);
        await _projectRepository.AddUserToProjectAsync("5", 1);

        Project? project = await _context.Projects.FindAsync(1);

        Assert.NotNull(project);
        var actual = project.Status;
        var expected = Status.Closed;

        Assert.Equal(expected, actual);
        Assert.NotNull(project.Participants);
        Assert.Equal(5, project.Participants.Count());
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

