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

public class UserController(
    IUserRepository repository, UserManager<User> userManager, 
    ITokenService tokenService, IPhotoService photoService, IMapper mapper) : BaseApiController
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
        User? user = await repository.GetUserById(User.GetUserId());
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
    

    [Authorize]
    [HttpPost("upload-photo")]
    public async Task<ActionResult> UploadPhoto(IFormFile file)
    {
        var user = await repository.GetUserById(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        var result = await photoService.AddPhoto(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new ProfilePhoto {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            UserId = user.Id
        };

        ProfilePhoto? currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        
        user.Photos.Add(photo);

        if (!await repository.Complete()) return BadRequest("Failed to set this as main photo");

        return Ok(mapper.Map<PhotoDto>(photo));
    }

    [Authorize]
    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await repository.GetUserByUsername(User.GetUsername());
        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null) return BadRequest("Could not find photo by its id");
        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (!await repository.Complete()) return BadRequest("Failed to set this as your main photo");
        return NoContent();
    }

    [Authorize]
    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await repository.GetUserById(User.GetUserId());
        if (user == null) return BadRequest("Could not find user");

        ProfilePhoto? photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null) return BadRequest("Could not find photo");
        if (photo.IsMain) return BadRequest("Cannot delete your main photo");

        if (photo.PublicId != null) 
        {
            var result = await photoService.DeletePhoto(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (!await repository.Complete()) return BadRequest("Failed to delete photo");

        return NoContent();
    }
}