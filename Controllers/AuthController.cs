using API.Dtos.Request;
using API.Dtos.Response;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AuthController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
    {
        if (loginDto.UserNameOrEmail == string.Empty) 
            return BadRequest("You must provide a username or email");
        
        User? user = null;

        if (loginDto.UserNameOrEmail.Contains('@')) 
        {
            user = await userManager.Users
                .Where(user => user.NormalizedEmail== loginDto.UserNameOrEmail.ToUpper())
                .FirstOrDefaultAsync();
        } else 
        {
            user = await userManager.Users
                .Where(user => user.NormalizedUserName == loginDto.UserNameOrEmail.ToUpper())
                .FirstOrDefaultAsync();
        }

        if (user == null || user.UserName == null) return Unauthorized("No user was found with the given username or email");

        if (!await userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized("Invalid password");

        var token = await tokenService.CreateToken(user);

        AccountDto accountDto = mapper.Map<AccountDto>(user);
        accountDto.Token = token;

        return Ok(accountDto);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AccountDto>> Register(RegistrationDto registrationDto)
    {
        if (await UsernameExists(registrationDto.UserName)) return BadRequest("This Username is already taken");
        if (await EmailExists(registrationDto.Email)) return BadRequest("This Email address is already taken");

        var user = mapper.Map<User>(registrationDto);

        user.UserName = registrationDto.UserName.ToLower();
        user.Email = registrationDto.Email.ToLower();

        var result = await userManager.CreateAsync(user, registrationDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        await userManager.AddToRoleAsync(user, "Member");

        var token = await tokenService.CreateToken(user);

        AccountDto accountDto = mapper.Map<AccountDto>(user);
        accountDto.Token = token;

        return Ok(accountDto);
    }

    private async Task<bool> UsernameExists(string username)
    {
        return await userManager.Users.AnyAsync(user => user.NormalizedUserName == username.ToUpper());
    }

    private async Task<bool> EmailExists(string email)
    {
        return await userManager.Users.AnyAsync(user => user.NormalizedEmail == email.ToUpper());
    }
}
