namespace ProjectBank.Infrastructure;

public class UserRepository : IUserRepository
{
    public readonly IProjectBankContext _context;

    public UserRepository(IProjectBankContext context)
    {
        _context = context;
    }
    public async Task<UserDTO> CreateUserAsync(CreateUserDTO user)
    {

        var existUser = await _context.Users.FindAsync(user.Id);

        if (existUser == null)
        {
            var entity = new User(user.Id, user.Name, user.Role);

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();

            return new UserDTO(
                entity.Id,
                entity.Name
            );
        }


        return new UserDTO(
                existUser.Id,
                existUser.Name
            );
    }

    public async Task<Option<UserDetailsDTO>> ReadUserByIdAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            return new UserDetailsDTO(user.Id, user.Name, user.Role);
        }
        else return null;
    }
}
