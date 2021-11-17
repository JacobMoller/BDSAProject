namespace ProjectBank.Core;
public interface IProjectRepository
{
    Task<ProjectDTO> CreateProjectAsync(ProjectCreateDTO project);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<ProjectDTO> ReadProjectAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserAsync(int userId);
    Task<Response> UpdateProjectAsync(ProjectDTO project);
    Task<Response> DeleteProjectAsync(int projectId);
}