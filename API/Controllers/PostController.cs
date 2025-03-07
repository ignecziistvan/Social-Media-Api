using API.Dtos.Response;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PostController(IPostRepository repository, IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetLatestPosts([FromQuery] int? count) 
    {
        return Ok(await repository.GetLatestPosts(count));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id) 
    {
        PostDto? post = await repository.GetPost(id);

        if (post == null) return NotFound("Post was not found");
        
        return Ok(post);
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsOfUser(string userName) 
    {
        User? user = await userRepository.GetUserByUserNameAsNonDto(userName);

        if (user == null) return NotFound("User was not found");

        return Ok(await repository.GetPostsOfUser(user));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] string text)
    {
        string? userName = User.Identity?.Name;

        if (userName == null) return Forbid("No identity");

        User? user = await userRepository.GetUserByUserNameAsNonDto(userName);

        if (user == null) return NotFound("User was not found");

        repository.CreatePost(
            new Post
            {
                UserId = user.Id,
                Text = text
            }
        );
        
        if (await repository.Complete()) return Ok();

        return BadRequest("Failed to create Post");
    }
}
