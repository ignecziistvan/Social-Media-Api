using API.Dtos.Response;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController(IUserRepository repository, IMapper mapper) : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] string username)
    {
        User? user = await repository.GetUserByUsername(username);
        if (user == null) return NotFound("Could not find user");

        return Ok(mapper.Map<UserDto>(user));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(
        [FromQuery] PaginationParams paginationParams
    )
    {
        var users = await repository.GetAllUsers(paginationParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}
