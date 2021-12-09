namespace ProjectBank.Core;
public interface IProjectRepository
{
    Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO project);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<ProjectDTO> ReadProjectByIdAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByUserIdAsync(string userId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagIdAsync(int tagId);
    Task EditProjectAsync(UpdateProjectDTO project);
    Task CloseProjectByIdAsync(int projectId);
    Task DeleteProjectByIdAsync(int projectId);
    Task AddUserToProjectAsync(string userId, int projectId);
}