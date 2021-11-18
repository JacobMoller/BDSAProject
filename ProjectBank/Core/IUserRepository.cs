namespace ProjectBank.Core;

public interface IUserRepository
{
    async int createUser(CreateUserDTO user);
    async void updateUser(UpdateUserDTO user);
    async void deleteUser(int id);
    async UserDTO getUserById(int id);
    async UserDTO getUserByEmail(string email);
    async UserDTO getSupervisorOnProject(int projectId);
    async ICollection<UserDTO> getAllUsers();
    async ICollection<UserDTO> getAllUsersByRole(Role role);
    async ICollection<UserDTO> getUsersAssignedToProject(int projectId);
}