namespace ProjectBank.Core;

public interface IUserRepository
{
    Task<UserDTO> CreateUserAsync(CreateUserDTO user);
    Task<Option<UserDetailsDTO>> ReadUserByIdAsync(string id);
}