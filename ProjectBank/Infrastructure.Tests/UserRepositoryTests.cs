namespace ProjectBank.Infrastructure.Tests;

public class UserRepositoryTests : ContextSetup, IDisposable
{
    [Fact]
    public async Task CreateUser_given_CreateUserDTO_returns_UserDTO()
    {
        //Needs to return a ProjectDetailedDTO
        var user = new CreateUserDTO
        {
            Name = "Alice",
            Email = "email@email.com",
            Password = "Password123",
            Role = Role.Student
        };

        var created = await _userRepository.CreateUserAsync(user);

        //Assert
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}