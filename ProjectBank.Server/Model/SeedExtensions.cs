using ProjectBank.Infrastructure;

namespace ProjectBank.Server.Model;

public static class SeedExtensions
{
    public static IHost Seed(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();

            SeedProjects(context);
        }
        return host;
    }

    private static void SeedProjects(ProjectBankContext context)
    {
        context.Database.Migrate();

        if (!context.Projects.Any())
        {
            var Algo = new Tag("Algo");
            var DMAT = new Tag("DMAT");
            var Disys = new Tag("Disys");


            
             var Hans = new User("Hans", "HansTheStudent@gmail.com", "2222", Role.Student);
             var Grete = new User("Grete", "GreteTheStudent@gmail.com", "5555", Role.Student);

             
            context.Users.AddRange(
                new User("Bob", "BobTheSupervisor@gmail.com", "1234", Role.Supervisor),
                new User("Alice", "AliceTheSupervisor@gmail.com", "9876", Role.Supervisor),
                Hans, 
                Grete
            );

            context.Projects.AddRange(
                new Project("Super Fun Project", Status.Active, 1) { Tags = new List<Tag>() { Algo, DMAT } },
                new Project("Super Closed Project", Status.Closed, 2) { Tags = new List<Tag>() { DMAT, DMAT } },
                new Project("Super Participants Project", Status.Active, 1) { Tags = new List<Tag>() { Algo, Disys }, Participants = new List<User>() { Hans, Grete } }
            );

            context.SaveChanges();
        }
    }
}