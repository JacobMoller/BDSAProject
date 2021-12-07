using Microsoft.AspNetCore.Mvc;
using ProjectBank.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

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
}

