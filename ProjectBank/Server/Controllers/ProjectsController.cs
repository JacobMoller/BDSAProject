using Microsoft.AspNetCore.Mvc;
using ProjectBank.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using ProjectBank.Server.Model;

namespace ProjectBank.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectsController : ControllerBase
{
    private IProjectRepository _projectRepository;

    public ProjectsController(IProjectRepository repo)
    {
        _projectRepository = repo;
    }

    [HttpGet]
    public async Task<IEnumerable<ProjectDTO>> Get()
    {
        return await _projectRepository.ReadAllAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectDTO>> Get(int id) => (await _projectRepository.ReadProjectByIdAsync(id)).ToActionResult();
}

