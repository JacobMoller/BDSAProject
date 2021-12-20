namespace ProjectBank.Server.Controllers;

[Authorize(Roles = "Student")]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class StudentController : ControllerBase
{
    public Func<string> GetObjectId;
    private IProjectRepository _projectRepository;

    public StudentController(IProjectRepository repo)
    {
        _projectRepository = repo;
        GetObjectId = () =>
            User.GetObjectId() == null ? "1" : User.GetObjectId();
    }

    [HttpPut("{projectId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int projectId)
        => (await _projectRepository.AddUserToProjectAsync(GetObjectId(), projectId)).ToActionResult();

    [HttpGet]
    public async Task<IEnumerable<ProjectDTO>> Get()
        => await _projectRepository.ReadProjectsByStudentIdAsync(GetObjectId());
}

