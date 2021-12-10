namespace ProjectBank.Core;
public interface IProjectRepository
{
    Task<ProjectDTO> CreateProjectAsync(CreateProjectDTO project);
    Task<IReadOnlyCollection<ProjectDTO>> ReadAllAsync();
    Task<Option<ProjectDTO>> ReadProjectByIdAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsBySupervisorIdAsync(string userId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByStudentIdAsync(string studentId);
    Task<IReadOnlyCollection<ProjectDTO>> ReadProjectsByTagIdAsync(int tagId);
    Task EditProjectAsync(int projectId, UpdateProjectDTO project);
    Task CloseProjectByIdAsync(int projectId);
    Task DeleteProjectByIdAsync(int projectId);
    Task AddUserToProjectAsync(string userId, int projectId);
}