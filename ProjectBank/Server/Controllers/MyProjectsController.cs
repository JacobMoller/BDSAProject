using Microsoft.AspNetCore.Mvc;
using ProjectBank.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using ProjectBank.Server.Model;

namespace ProjectBank.Server.Controllers;

[Authorize(Roles="Supervisor")] 
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MyProjectsController : ControllerBase
{
    private IProjectRepository _projectRepository;

    public MyProjectsController(IProjectRepository repo)
    {
        _projectRepository = repo;
    }
[ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<List<ProjectDTO>> Get()
        => (await _projectRepository.ReadProjectsByUserIdAsync(User.GetObjectId())).ToList();
}

