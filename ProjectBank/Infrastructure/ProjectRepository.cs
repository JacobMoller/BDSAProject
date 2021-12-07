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
            CreationDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            Participants = new List<User>()
        };
        if (project.Tags != null)
        {
            entity.Tags = await SetTagsAsync(project.Tags);
        }

        _context.Projects.Add(entity);

        await _context.SaveChangesAsync();

        return new ProjectDTO(
            entity.Id,
            entity.Title,
            entity.Status,
            entity.UserId
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
        //Should projectDTO title be nullable? We need it for create but not for update
        entity.Title = project.Title;
        entity.Description = project.Description;
        entity.UpdatedDate = DateTime.Now;
        if (project.Tags != null)
        {
            entity.Tags = await SetTagsAsync(project.Tags);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
    {
        return await _context.Projects.Select(project => new ProjectDTO(
               project.Id,
               project.Title,
               project.Status,
               project.UserId))
               .ToListAsync();
    }

    public async Task<ProjectDTO> ReadProjectByIdAsync(int projectId)
    {

        var entity = await _context.Projects.FindAsync(projectId);
        if (entity != null)
        {
            return new ProjectDTO(
                entity.Id,
                entity.Title,
                entity.Status,
                entity.UserId
            );
        }
        else
        {
            return null;
        }
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagIdAsync(int tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        return (from project in tag.Projects
                select new ProjectDTO(
                project.Id,
                project.Title,
                project.Status,
                project.UserId)).ToList();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserIdAsync(int userId)
    {
        return await _context.Projects.Where(project => project.UserId == userId).Select(project => new ProjectDTO(
               project.Id,
               project.Title,
               project.Status,
               project.UserId))
               .ToListAsync();
    }

    public async Task UpdateProjectStatusByIdAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);
        if (entity != null)
        {
            entity.Status = Status.Closed;
            entity.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddUserToProjectAsync(int userId, int projectId)
    {
        var user = await _context.Users.FindAsync(userId);
        var project = await _context.Projects.FindAsync(projectId);
        if (user != null && project != null)
        {
            project.Participants.Add(user);
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