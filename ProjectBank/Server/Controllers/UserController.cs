using Microsoft.AspNetCore.Authorization;

namespace ProjectBank.Server.Controllers;

public class UserController : ControllerBase
{

    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {

        _repository = repository;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(UserDTO), 201)]
    public async Task<IActionResult> Post(CreateUserDTO user)
    {
        var created = await _repository.createUser(user);

        return CreatedAtAction(nameof(Get), new { created.UserId }, created);
    }


}