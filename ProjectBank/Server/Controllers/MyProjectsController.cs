namespace ProjectBank.Server.Controllers;

[Authorize(Roles = "Supervisor")]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MyProjectsController : ControllerBase
{
    private IProjectRepository _projectRepository;
    public Func<string> GetObjectId;

    public MyProjectsController(IProjectRepository repo)
    {
        _projectRepository = repo;
        GetObjectId = () => User.GetObjectId();
    }
    
    [Authorize(Roles = "Supervisor")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<List<ProjectDTO>> Get()
            => (await _projectRepository.ReadProjectsBySupervisorIdAsync(GetObjectId())).ToList();
}

