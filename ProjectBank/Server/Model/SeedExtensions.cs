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

        var userRepository = new UserRepository(context);
        var ProjectRepository = new ProjectRepository(context);

        await userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Alice",
            Email = "alice@alice.com",
            Password = "AlicesPassword",
            Role = Role.Supervisor
        });
        await userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Bob",
            Email = "bob@bob.com",
            Password = "BobsPassword",
            Role = Role.Supervisor
        });
        await userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Charlie",
            Email = "charlie@charlie.com",
            Password = "CharliesPassword",
            Role = Role.Student
        });
        await userRepository.CreateUserAsync(new CreateUserDTO
        {
            Name = "Dave",
            Email = "dave@dave.com",
            Password = "DavesPassword",
            Role = Role.Student
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Fun Project",
            Description = "Fun",
            UserId = 1,
            Tags = new List<string>() { "Algorithm", "Economy" }
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Participants Project",
            Description = "People",
            UserId = 1,
            Tags = new List<string>() { "Algorithm", "Math" }
        });
        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Closed Project",
            Description = "I was a fun project",
            UserId = 2,
            Tags = new List<string>() { "Math" }
        });
        await ProjectRepository.AddUserToProjectAsync(3, 2);
        await ProjectRepository.AddUserToProjectAsync(4, 2);
        await ProjectRepository.CloseProjectByIdAsync(3);
        await ProjectRepository.EditProjectAsync(new UpdateProjectDTO()
        {
            Id = 1,
            Title = "Super Fun Project",
            Description = "Fun",
            Tags = new List<string>() { "hej", "farvel" }
        });
    }
}