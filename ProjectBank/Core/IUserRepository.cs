namespace ProjectBank.Core;

public interface IUserRepository
{
    Task<UserDTO> CreateUserAsync(CreateUserDTO user);
    Task UpdateUserAsync(UpdateUserDTO user);
    Task DeleteUserAsync(int id);
    Task<UserDTO> ReadUserByIdAsync(int id);
    Task<UserDTO> ReadUserByEmailAsync(string email);
    Task<UserDTO> ReadSupervisorOnProjectByIdAsync(int projectId);
    Task<IReadOnlyCollection<UserDTO>> ReadUsersAsync();
    Task<IReadOnlyCollection<UserDTO>> ReadUsersByRoleAsync(Role role);
    Task<IReadOnlyCollection<UserDTO>> ReadUsersAssignedToProjectAsync(int projectId);
}