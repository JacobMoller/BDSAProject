namespace ProjectBank.Core;

public interface IUserRepository
{
    Task<int> createUser(CreateUserDTO user);
    void updateUser(UpdateUserDTO user);
    void deleteUser(int id);
    Task<UserDTO> getUserById(int id);
    Task<UserDTO> getUserByEmail(string email);
    Task<UserDTO> getSupervisorOnProject(int projectId);
    Task<ICollection<UserDTO>> getAllUsers();
    Task<ICollection<UserDTO>> getAllUsersByRole(Role role);
    Task<ICollection<UserDTO>> getUsersAssignedToProject(int projectId);
}