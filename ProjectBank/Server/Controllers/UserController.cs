using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

namespace ProjectBank.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{

    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {

        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<UserDTO>> Get()
        => await _repository.ReadUsersAsync();

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> Get(int id)
        => (await _repository.ReadUserByIdAsync(id)).ToActionResult();

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(UserDetailsDTO), 201)]
    public async Task<ActionResult<UserDetailsDTO>> Post(CreateUserDTO user)
    {
        var created = await _repository.CreateUserAsync(user);

        return CreatedAtRoute(nameof(Get), new { created.Id }, created);
    }


}