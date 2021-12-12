namespace Server.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ProjectBankContext>));

            if (dbContext != null)
            {
                services.Remove(dbContext);
            }

            /* Overriding policies and adding Test Scheme defined in TestAuthHandler */
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Test")
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            var connection = new SqliteConnection("Filename=:memory:");

            services.AddDbContext<ProjectBankContext>(options =>
            {
                options.UseSqlite(connection);
            });

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();
            appContext.Database.OpenConnection();
            appContext.Database.EnsureCreated();

            Seed(appContext);
        });

        builder.UseEnvironment("Integration");

        return base.CreateHost(builder);
    }

    private async static Task Seed(ProjectBankContext context)
    {
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
            SupervisorId = "1",
            Tags = new List<string>() { "Algorithm", "Math" }
        });

        await ProjectRepository.CreateProjectAsync(new CreateProjectDTO
        {
            Title = "Super Enjoyable Project",
            Description = "Very cool description",
            SupervisorId = "1",
            Tags = new List<string>() { "DMAT" }
        });

        await ProjectRepository.AddUserToProjectAsync("1", 2);
        await ProjectRepository.AddUserToProjectAsync("2", 2);
    }
}