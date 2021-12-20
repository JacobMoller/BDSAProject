namespace ProjectBank.Server.Controllers;

[Authorize(Roles = "Student")]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ApplyController : ControllerBase
{
    private IProjectRepository _projectRepository;

    public ApplyController(IProjectRepository repo)
    {
        _projectRepository = repo;
    }

    [HttpPut("{projectId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(int projectId, [FromBody] ProjectDTO project)
        => (await _projectRepository.AddUserToProjectAsync(User.GetObjectId(), projectId)).ToActionResult();
}

