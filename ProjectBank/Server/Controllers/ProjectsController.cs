﻿using Microsoft.AspNetCore.Mvc;
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

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
        => (await _projectRepository.ReadProjectByIdAsync(id)).ToActionResult();

    [HttpPost]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(CreateProjectDTO project)
    {
        project.UserId = User.GetObjectId();

        var created = await _projectRepository.CreateProjectAsync(project);

        return CreatedAtRoute(nameof(Get), new { created.Id }, created);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Put(UpdateProjectDTO character)
            => await _projectRepository.EditProjectAsync(character);

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Delete(int id)
        => await _projectRepository.DeleteProjectByIdAsync(id);
}

