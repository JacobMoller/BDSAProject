namespace ProjectBank.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    public Func<string> GetObjectId;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
        GetObjectId = () => User.GetObjectId();
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDTO>> Get(string id)
        => (await _repository.ReadUserByIdAsync(id)).ToActionResult();

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(UserDetailsDTO), 201)]
    public async Task<IActionResult> Post(CreateUserDTO user)
    {
        user.Id = GetObjectId();

        var created = await _repository.CreateUserAsync(user);


        return CreatedAtAction(nameof(Get), new { created.Id }, created);
    }
}