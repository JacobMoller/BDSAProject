namespace ProjectBank.Core;
public interface IProjectRepository
{
    Task<ProjectDTO> CreateProjectAsync(ProjectCreateDTO project);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<ProjectDTO> ReadProjectByIdAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserAsync(int userId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagAsync(int tagId);
    Task<Response> EditProjectAsync(ProjectDTO project);
    Task<Response> UpdateProjectStatusByIdAsync(int projectId);
    Task<Response> DeleteProjectByIdAsync(int projectId);
}