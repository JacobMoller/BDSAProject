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
        context.Database.Migrate();
        await context.Database.MigrateAsync();

        if (!await context.Projects.AnyAsync())
        {
            var Algo = new Tag("Algo");
            var DMAT = new Tag("DMAT");
            var Disys = new Tag("Disys");

            var Charlie = new User("Charlie", "CharlieTheStudent@gmail.com", "2222", Role.Student);
            var Dave = new User("Dave", "DaveTheStudent@gmail.com", "5555", Role.Student);


            context.Users.AddRange(
                new User("Bob", "BobTheSupervisor@gmail.com", "1234", Role.Supervisor),
                new User("Alice", "AliceTheSupervisor@gmail.com", "9876", Role.Supervisor),
                Charlie,
                Dave

            );

            context.Projects.AddRange(
                new Project("Super Fun Project", Status.Active, 1) { Tags = new List<Tag>() { Algo, DMAT } },
                new Project("Super Closed Project", Status.Closed, 2) { Tags = new List<Tag>() { DMAT, DMAT } },
                new Project("Super Participants Project", Status.Active, 1) { Tags = new List<Tag>() { Algo, Disys }, Participants = new List<User>() { Dave, Charlie } }

            );

            await context.SaveChangesAsync();
        }
    }    
     
}