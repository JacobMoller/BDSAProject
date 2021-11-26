namespace ProjectBank.Infrastructure.Tests;

public class UserRepositoryTests : ContextSetup, IDisposable
{
    [Fact]
    public async Task CreateUser_given_CreateUserDTO_returns_UserDTO()
    {
        var user = new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Student
        };

        var expected = new UserDTO(1, "Alice");
        var actual = await _userRepository.CreateUserAsync(user);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task DeleteUser_given_userId_deletes_user()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Student
        });

        await _userRepository.DeleteUserAsync(1);

        Assert.Null(await _context.Users.FindAsync(1));

    }

    [Fact]
    public async Task ReadSupervisorOnProjectByIdAsync_given_projectId_returns_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Supervisor
        });

        await _projectRepository.CreateProjectAsync(new CreateProjectDTO()
        {
            Title = "Algorithm",
            Description = "Cool algorithms",
            UserId = 1
        });

        var expected = new UserDTO(1, "Alice");
        var actual = await _userRepository.ReadSupervisorOnProjectByIdAsync(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadUserByEmailAsync_given_email_returns_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Supervisor
        });

        var expected = new UserDTO(1, "Alice");
        var actual = await _userRepository.ReadUserByEmailAsync("email@email.com");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadUserByIdAsync_given_Id_returns_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Supervisor
        });

        var expected = new UserDTO(1, "Alice");
        var actual = await _userRepository.ReadUserByIdAsync(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadUsersAssignedToProjectAsync_given_projectId_returns_list_of_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "alice@alice.com",
            Password = "Password123",
            Role = Role.Supervisor
        });
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Bob",
            Email = "bob@bob.com",
            Password = "Password123",
            Role = Role.Supervisor
        });
        await _projectRepository.CreateProjectAsync(new CreateProjectDTO()
        {
            Title = "Algorithm",
            Description = "Cool algorithms",
            UserId = 1
        });
        await _projectRepository.AddUserToProjectAsync(1, 1);
        await _projectRepository.AddUserToProjectAsync(2, 1);

        var expected = new List<UserDTO>() { new UserDTO(1, "Alice"), new UserDTO(2, "Bob") };
        var actual = await _userRepository.ReadUsersAssignedToProjectAsync(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadUsersAsync_returns_list_of_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "alice@alice.com",
            Password = "Password123",
            Role = Role.Supervisor
        });
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Bob",
            Email = "bob@bob.com",
            Password = "Password123",
            Role = Role.Supervisor
        });
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Charlie",
            Email = "charlie@charlie.com",
            Password = "Password123",
            Role = Role.Supervisor
        });


        var expected = new List<UserDTO>() { new UserDTO(1, "Alice"), new UserDTO(2, "Bob"), new UserDTO(3, "Charlie") };
        var actual = await _userRepository.ReadUsersAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadUsersByRoleAsync_given_role_returns_list_of_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "alice@alice.com",
            Password = "Password123",
            Role = Role.Student
        });
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Bob",
            Email = "bob@bob.com",
            Password = "Password123",
            Role = Role.Supervisor
        });
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Charlie",
            Email = "charlie@charlie.com",
            Password = "Password123",
            Role = Role.Supervisor
        });

        var expectedSupervisors = new List<UserDTO>() { new UserDTO(2, "Bob"), new UserDTO(3, "Charlie") };
        var actualSupervisors = await _userRepository.ReadUsersByRoleAsync(Role.Supervisor);
        var expectedStudent = new List<UserDTO>() { new UserDTO(1, "Alice") };
        var actualStudent = await _userRepository.ReadUsersByRoleAsync(Role.Student);

        Assert.Equal(expectedSupervisors, actualSupervisors);
        Assert.Equal(expectedStudent, actualStudent);
    }


    public void Dispose()
    {
        _context.Dispose();
    }
}