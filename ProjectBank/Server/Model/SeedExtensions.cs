using ProjectBank.Infrastructure;

namespace ProjectBank.Server.Model;

public static class SeedExtensions
{
    public async static Task<IHost> Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

            await SeedProjects(context);
        }
        return host;
    }

    private async static Task SeedProjects(ProjectBankContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();
        await context.Database.MigrateAsync();

        var userRepository = new UserRepository(context);
        var ProjectRepository = new ProjectRepository(context);

        await userRepository.CreateUserAsync(new CreateUserDTO
        {
            Id = "1",
            Name = "Charlie",
            Role = Role.Student
        });
        await userRepository.CreateUserAsync(new CreateUserDTO
        {
            Id = "2",
            Name = "Dave",
            Role = Role.Student
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Fun Project",
            Description = "Fun",
            SupervisorId = "1",
            Tags = new List<string>() { "Algorithm", "Economy" }
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Participants Project",
            Description = "People",
            SupervisorId = "2",
            Tags = new List<string>() { "Algorithm", "Math" }
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Closed Project",
            Description = "I was a fun project",
            SupervisorId = "2",
            Tags = new List<string>() { "Math" }
        });
        await ProjectRepository.AddUserToProjectAsync("1", 2);
        await ProjectRepository.AddUserToProjectAsync("2", 2);
        await ProjectRepository.CloseProjectByIdAsync(3);
        await ProjectRepository.EditProjectAsync(1, new UpdateProjectDTO()
        {
            Id = 1,
            Title = "Super Fun Project",
            Description = "Fun",
            Tags = new List<string>() { "hej", "farvel" }
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Enjoyable Project",
            Description = "Very cool description",
            SupervisorId = "2",
            Tags = new List<string>() { "DMAT" }
        });
    }
}