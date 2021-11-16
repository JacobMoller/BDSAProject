namespace ProjectBank.Core;

public interface IUserRepository
{
    int createUser(CreateUserDTO user);
    void updateUser(UpdateUserDTO user);
    void deleteUser(int id);
    UserDTO getUserById(int id);
    UserDTO getUserByEmail(string email);
    UserDTO getSupervisorOnProject(int projectId);
    ICollection<UserDTO> getAllUsers();
    ICollection<UserDTO> getAllUsersByRole(Role role);
    ICollection<UserDTO> getUsersAssignedToProject(int projectId);
}