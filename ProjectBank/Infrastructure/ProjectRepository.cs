using System.Linq;

namespace ProjectBank.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    public readonly IProjectBankContext _context;

    public ProjectRepository(IProjectBankContext context)
    {
        _context = context;
    }
    public async Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO project)
    {
        var entity = new Project(project.Title, Status.Active, project.UserId)
        {
            Description = project.Description,
            CreationDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Tags = project.Tags != null ? await SetTagsAsync(project.Tags) : new List<Tag>(),
            Participants = new List<User>()

        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return new ProjectDTO(
            entity.Id,
            entity.Title,
            entity.Status.ToString(),
            entity.UserId,
            entity.Description,
            entity.CreationDate,
            entity.UpdatedDate,
            entity.Tags.Select(tag => new string(tag.Name)).ToList().AsReadOnly(),
            new List<UserDTO>()
        );
    }

    public async Task DeleteProjectByIdAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);
        if (entity != null)
        {
            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditProjectAsync(UpdateProjectDTO project)
    {
        var entity = await _context.Projects.FindAsync(project.Id);
        if (entity != null)
        {
            entity.Title = project.Title;
            entity.Description = project.Description;
            entity.UpdatedDate = DateTime.UtcNow;
            if (project.Tags != null && project.Tags.Count > 0)
            {
                entity.Tags = await SetTagsAsync(project.Tags);
            }
            else
            {
                entity.Tags = new List<Tag>();
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
    {
        return await (_context.Projects.Where(project => project.Status == Status.Active).Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.UserId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(tag => new string(tag.Name)).ToList(),
            project.Participants.Select(user => new UserDTO(user.Id, user.Name)).ToList()
            ))).ToListAsync();
    }

    public async Task<Option<ProjectDTO>> ReadProjectByIdAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);
        if (entity != null)
        {
            return new ProjectDTO(
                entity.Id,
                entity.Title,
                entity.Status.ToString(),
                entity.UserId,
                entity.Description,
                entity.CreationDate,
                entity.UpdatedDate,
                entity.Tags.Select(tag => new string(tag.Name)).ToList(),
                entity.Participants.Select(user => new UserDTO(user.Id, user.Name)).ToList()
            );
        }
        else
        {
            return null; //TODO: throw new CouldNotFindEntityInDatabase
        }

    }
    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagIdAsync(int tagId)
    {
        var searchTag = await _context.Tags.FindAsync(tagId);
        return await (_context.Projects.Where(p => searchTag.Projects.Contains(p) && p.Status == Status.Active).Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.UserId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(t => new string(t.Name)).ToList(),
            project.Participants.Select(u => new UserDTO(u.Id, u.Name)).ToList()
            ))).ToListAsync();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserIdAsync(string userId)
    {
        return await (_context.Projects.Where(project => project.UserId == userId).Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.UserId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(t => new string(t.Name)).ToList().AsReadOnly(),
            project.Participants.Select(u => new UserDTO(u.Id, u.Name)).ToList().AsReadOnly()
            ))).ToListAsync();
    }

    public async Task CloseProjectByIdAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);
        if (entity != null)
        {
            entity.Status = Status.Closed;
            entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddUserToProjectAsync(string userId, int projectId)
    {
        var user = await _context.Users.FindAsync(userId);
        var project = await _context.Projects.FindAsync(projectId);
        if (user != null && project != null)
        {
            if (project.Participants.Count() < 5)
            {
                if (project.Participants.Count() == 4)
                {
                    project.Status = Status.Closed;
                }
                project.Participants.Add(user);
            }
            project.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    private async Task<ICollection<Tag>> SetTagsAsync(ICollection<string> tags)
    {
        var listToReturn = new List<Tag>();
        var listFromDB = _context.Tags.Select(t => t.Name);
        foreach (var tag in tags)
        {
            if (!listFromDB.Contains(tag))
            {
                listToReturn.Add(new Tag(tag));
            }
            else
            {
                var existingTag = await _context.Tags.Where(t => t.Name == tag).SingleOrDefaultAsync();
                if (existingTag != null)
                {
                    listToReturn.Add(existingTag);
                }
            }
        }
        return listToReturn;
    }
}
