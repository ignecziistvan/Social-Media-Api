using API.Dtos.Request;
using API.Dtos.Response;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController(IUserRepository repository, UserManager<User> userManager, ITokenService tokenService, IMapper mapper) : BaseApiController
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

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<AccountDto>> UpdateUser(UpdateUserDto dto)
    {
        User? user = await userManager.FindByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Could not find user");

        if (!string.IsNullOrEmpty(dto.Username)) {
            User? foundUser = await userManager.FindByNameAsync(dto.Username.ToLower());
            if (foundUser != null) return BadRequest("Username already exists");

            user.UserName = dto.Username.ToLower();
        }

        if (!string.IsNullOrEmpty(dto.Email)) {
            User? foundUser = await userManager.FindByEmailAsync(dto.Email.ToLower());
            if (foundUser != null) return BadRequest("Email already exists");

            user.Email = dto.Email.ToLower();
        }

        if (!string.IsNullOrEmpty(dto.Password)) {
            if (dto.Password.Length < 6 || dto.Password.Length > 32) 
                return BadRequest("Password length must be between 6 and 32");

            if (!dto.Password.Any(char.IsUpper))
                return BadRequest("Password must contain at least one uppercase letter");

            if (!dto.Password.Any(char.IsLower))
                return BadRequest("Password must contain at least one lowercase letter");

            if (string.IsNullOrEmpty(dto.OldPassword)) return BadRequest("You must provide your previous password");

            if (!await userManager.CheckPasswordAsync(user, dto.OldPassword)) return BadRequest("Invalid old password");

            var passwordChangeResult = await userManager.ChangePasswordAsync(user, dto.OldPassword, dto.Password);

            if (!passwordChangeResult.Succeeded) 
                return BadRequest(passwordChangeResult.Errors);
        }

        if (!string.IsNullOrEmpty(dto.Firstname)) {
            user.Firstname = dto.Firstname;
        }

        if (!string.IsNullOrEmpty(dto.Lastname)) {
            user.Lastname = dto.Lastname;
        }

        if (dto.DateOfBirth.HasValue) {
            user.DateOfBirth = dto.DateOfBirth.Value;
        }

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        AccountDto accountDto = mapper.Map<AccountDto>(user);
        accountDto.Token = await tokenService.CreateToken(user);

        return Ok(accountDto);
    }

}
