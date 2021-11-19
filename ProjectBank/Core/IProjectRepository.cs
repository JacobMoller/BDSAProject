namespace ProjectBank.Core;
public interface IProjectRepository
{
    Task<ProjectDTO> CreateProjectAsync(ProjectCreateDTO project);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<ProjectDTO> ReadProjectAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserAsync(int userId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTag(int tagId);
    Task<Response> EditProjectAsync(ProjectDTO project);
    Task<Response> UpdateProjectStatusAsync(int projectId);
    Task<Response> DeleteProjectAsync(int projectId);
}