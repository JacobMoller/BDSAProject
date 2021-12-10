namespace ProjectBank.Infrastructure.Tests;

public class UserRepositoryTests : ContextSetup, IDisposable
{
    [Fact]
    public async Task CreateUser_given_CreateUserDTO_returns_UserDTO()
    {
        var user = new CreateUserDTO { Id = "1", Name = "Alice", Role = Role.Student };

        var expected = new UserDTO("1", "Alice");
        var actual = await _userRepository.CreateUserAsync(user);

        Assert.Equal(expected, actual);
        Assert.Equal(new UserDTO("1", "Alice"), await _userRepository.CreateUserAsync(user));
    }

    [Fact]
    public async Task ReadUserByIdAsync_given_Id_returns_UserDTO()
    {
        await _userRepository.CreateUserAsync(new CreateUserDTO
        {
            Id = "1",
            Name = "Alice",
            Role = Role.Supervisor
        });

        var expected = new UserDetailsDTO("1", "Alice", "Supervisor");
        var actual = await _userRepository.ReadUserByIdAsync("1");

        Assert.Equal(expected, actual.Value);
        Assert.True((await _userRepository.ReadUserByIdAsync("100")).IsNone);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}