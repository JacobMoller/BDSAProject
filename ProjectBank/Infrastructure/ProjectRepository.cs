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
            Tags = project.Tags != null ? GetTags(project.Tags) : null,
        };

        _context.Projects.Add(entity);

        await _context.SaveChangesAsync();

        return new ProjectDTO(
            entity.Id,
            entity.Title,
            entity.Status,
            entity.UserId
        );
    }

    public async void DeleteProjectByIdAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);
        // make sure to give a proper response if null (http statuscode?)
        if (entity != null)
        {
            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async void EditProjectAsync(UpdateProjectDTO project)
    {
        var entity = await _context.Projects.FindAsync(project.Id);
        //Should projectDTO title be nullable? We need it for create but not for update
        entity.Title = project.Title;
        entity.Description = project.Description;
        if (project.Tags != null)
        {
            if (entity.Tags == null)
            {
                entity.Tags = new List<Tag>();
            }
            foreach (var tagName in project.Tags)
            {
                entity.Tags.Add(new Tag(tagName));
            }

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
        throw new NotImplementedException();
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

    public async void UpdateProjectStatusByIdAsync(int projectId)
    {
        var entity = await _context.Projects.FindAsync(projectId);
        if (entity != null)
        {
            entity.Status = Status.Closed;
        }
        await _context.SaveChangesAsync();
    }

    private static ICollection<Tag> GetTags(ICollection<string> tags)
    {
        var list = new List<Tag>();
        foreach (var tag in tags)
        {
            list.Add(new Tag(tag));
        }
        return list;
    }
}
