namespace ProjectBank.Infrastructure.Tests;

public class UserRepositoryTests : ContextSetup, IDisposable
{
    [Fact]
    public async Task CreateUser_given_CreateUserDTO_returns_UserDTO()
    {
        var user = new CreateUserDTO
        {
            Name = "Alice",
            Role = Role.Student
        };

        var expected = new UserDTO(1, "Alice");
        var actual = await _userRepository.CreateUserAsync(user);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ReadUserByIdAsync_given_Id_returns_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Role = Role.Supervisor
        });

        var expected = new UserDTO(1, "Alice");
        var actual = await _userRepository.ReadUserByIdAsync(1);

        Assert.Equal(expected, actual);
        Assert.Null(await _userRepository.ReadUserByIdAsync(100));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}