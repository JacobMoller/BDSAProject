namespace ProjectBank.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    public Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO project)
    {
        throw new NotImplementedException();
    }

    public void DeleteProjectByIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public void EditProjectAsync(ProjectDTO project)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDTO> ReadProjectByIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagAsync(int tagId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public void UpdateProjectStatusByIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }
}