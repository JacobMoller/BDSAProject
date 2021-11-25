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
        var entity = new User(user.Name, user.Email, user.Password)
        {
            Role = user.Role,
        };

        _context.Users.Add(entity);

        await _context.SaveChangesAsync();

        return new UserDTO(
            entity.Id,
            entity.Name
        );
    }

    public async void DeleteUserAsync(int userId)
    {
        var entity = await _context.Users.FindAsync(userId);
        // make sure to give a proper response if null (http statuscode?)
        if (entity != null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<UserDTO> ReadSupervisorOnProjectByIdAsync(int projectId)
    {
        var supervisorID = await _context.Projects.FindAsync(projectId);
        var user = await _context.Users.FindAsync(supervisorID);
        if (user != null)
        {
            return new UserDTO(
                user.Id,
                user.Name
            );
        }
        else return null;
    }

    public async Task<UserDTO> ReadUserByEmailAsync(string email)
    {
        var user = await _context.Users.Where(user => user.Email == email).SingleOrDefaultAsync();
        if (user != null)
        {
            return new UserDTO(user.Id, user.Name);
        }
        else return null;
    }

    public async Task<UserDTO> ReadUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            return new UserDTO(user.Id, user.Name);
        }
        else return null;
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadUsersAssignedToProjectAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        var listOfParticipants = new List<UserDTO>();
        foreach (var user in project.Participants)
        {
            listOfParticipants.Add(new UserDTO(user.Id, user.Name));
        }
        return listOfParticipants;
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadUsersAsync()
    {
        return await _context.Users.Select(user => new UserDTO(
            user.Id,
            user.Name)).ToListAsync();
    }

    public async Task<IReadOnlyCollection<UserDTO>> ReadUsersByRoleAsync(Role role)
    {
        return await _context.Users.Where(user => user.Role == role).Select(user => new UserDTO(
            user.Id,
            user.Name
        )).ToListAsync();
    }

    public async void UpdateUserAsync(UpdateUserDTO user)
    {
        //do we want this since all fields in user are init and cant be updated
    }
}
