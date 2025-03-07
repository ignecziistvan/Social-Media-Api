using API.Dtos.Response;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController(IUserRepository repository) : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] string username)
    {
        UserDto? user = await repository.GetUser(username);

        if (user == null) return NotFound("Could not find user");

        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await repository.GetAllUsers();
        return Ok(users);
    }
}
