using System.Linq;
using ProjectBank.Core;

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
        var entity = new Project(project.Title, Status.Active, project.SupervisorId)
        {
            Description = project.Description,
            CreationDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Tags = await SetTagsAsync(project.Tags),
            Participants = new List<User>()

        };

        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();

        return new ProjectDTO(
            entity.Id,
            entity.Title,
            entity.Status.ToString(),
            entity.SupervisorId,
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

        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task EditProjectAsync(int projectId, UpdateProjectDTO project)
    {
        var entity = await _context.Projects.Include(p => p.Tags).Include(p => p.Participants).FirstOrDefaultAsync(p => p.Id == projectId);
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
        //List of all projects?
        return await (_context.Projects.Where(project => project.Status == Status.Active).Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.SupervisorId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(tag => new string(tag.Name)).ToList(),
            project.Participants.Select(user => new UserDTO(user.Id, user.Name)).ToList()
            ))).ToListAsync();
    }

    public async Task<Option<ProjectDTO>> ReadProjectByIdAsync(int projectId)
    {
        var entity = await _context.Projects.Include(p => p.Tags).Include(p => p.Participants).FirstOrDefaultAsync(p => p.Id == projectId);
        if (entity != null)
        {
            var tags = entity.Tags != null ? entity.Tags.Select(tag => new string(tag.Name)).ToList() : new List<string>();
            var participants = entity.Participants != null ? entity.Participants.Select(user => new UserDTO(user.Id, user.Name)).ToList() : new List<UserDTO>();
            return new ProjectDTO(
                entity.Id,
                entity.Title,
                entity.Status.ToString(),
                entity.SupervisorId,
                entity.Description,
                entity.CreationDate,
                entity.UpdatedDate,
                tags,
                participants
            );
        }
        return null; //TODO: throw new CouldNotFindEntityInDatabase
    }
    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagIdAsync(int tagId)
    {
        var searchTag = await _context.Tags.FindAsync(tagId);
        return await (_context.Projects.Where(p => searchTag.Projects.Contains(p) && p.Status == Status.Active).Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.SupervisorId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(t => new string(t.Name)).ToList(),
            project.Participants.Select(u => new UserDTO(u.Id, u.Name)).ToList()
            ))).ToListAsync();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsBySupervisorIdAsync(string supervisorId)
    {
        return await (_context.Projects.Where(project => project.SupervisorId == supervisorId).Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.SupervisorId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(t => new string(t.Name)).ToList().AsReadOnly(),
            project.Participants.Select(u => new UserDTO(u.Id, u.Name)).ToList().AsReadOnly()
            ))).ToListAsync();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByStudentIdAsync(string studentId)
    {
        var student = await _context.Users.FindAsync(studentId);
        //TODO: make async
        return (student.Projects.Select(project => new ProjectDTO(
            project.Id,
            project.Title,
            project.Status.ToString(),
            project.SupervisorId,
            project.Description,
            project.CreationDate,
            project.UpdatedDate,
            project.Tags.Select(t => new string(t.Name)).ToList().AsReadOnly(),
            project.Participants.Select(u => new UserDTO(u.Id, u.Name)).ToList().AsReadOnly()
            ))).ToList();
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

    public async Task AddUserToProjectAsync(string studentId, int projectId)
    {
        var user = await _context.Users.FindAsync(studentId);
        var project = await _context.Projects.Include(p => p.Tags).Include(p => p.Participants).FirstOrDefaultAsync(p => p.Id == projectId);
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
            project.UpdatedDate = DateTime.UtcNow;
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
