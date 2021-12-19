namespace ProjectBank.Server.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectsController : ControllerBase
{
    public Func<string> GetObjectId;
    private IProjectRepository _projectRepository;

    public ProjectsController(IProjectRepository repo)
    {
        _projectRepository = repo;
        GetObjectId = () =>
            User.GetObjectId() == null ? "1" : User.GetObjectId();
    }

    [HttpGet]
    public async Task<IEnumerable<ProjectDTO>> Get()
    {
        return await _projectRepository.ReadAllAsync();
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
        => (await _projectRepository.ReadProjectByIdAsync(id)).ToActionResult();

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(CreateProjectDTO project)
    {
        project.SupervisorId = GetObjectId();

        var created = await _projectRepository.CreateProjectAsync(project);

        return CreatedAtAction(nameof(Get), new { created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectDTO project)
        => (await _projectRepository.EditProjectAsync(id, project)).ToActionResult();

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
        => (await _projectRepository.DeleteProjectByIdAsync(id)).ToActionResult();
}

