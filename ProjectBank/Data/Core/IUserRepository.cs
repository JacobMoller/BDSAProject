namespace ProjectBank.Data.Core;

public interface IUserRepository
{
    int createUser(CreateUserDTO user);
    void updateUser(UpdateUserDTO user);
    void deleteUser(int id);
    UserDTO getUserById(int id);
    UserDTO getUserByEmail(string email);
    UserDTO getSupervisorOnProject(int projectId);
    ICollection getAllUsers();
    ICollection getAllUsersByRole(Role role);
    ICollection getUsersAssignedToProject(int projectId);
}
