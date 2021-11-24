namespace ProjectBank.Core;
public interface IProjectRepository
{
    Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO project);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<ProjectDTO> ReadProjectByIdAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserIdAsync(int userId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagIdAsync(int tagId);
    void EditProjectAsync(UpdateProjectDTO project);
    void UpdateProjectStatusByIdAsync(int projectId);
    void DeleteProjectByIdAsync(int projectId);
}