namespace ProjectBank.Infrastructure;

public class UserRepository : IUserRepository
{
    public readonly IProjectBankContext _context;

    public UserRepository(IProjectBankContext context)
    {
        _context = context;
    }
    public Task<UserDTO> CreateUserAsync(CreateUserDTO user)
    {
        throw new NotImplementedException();
    }

    public void DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> ReadSupervisorOnProjectByIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> ReadUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> ReadUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<UserDTO>> ReadUsersAssignedToProjectAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<UserDTO>> ReadUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<UserDTO>> ReadUsersByRoleAsync(Role role)
    {
        throw new NotImplementedException();
    }

    public void UpdateUserAsync(UpdateUserDTO user)
    {
        throw new NotImplementedException();
    }
}
