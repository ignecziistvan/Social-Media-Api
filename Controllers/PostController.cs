using API.Dtos.Response;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PostController(IPostRepository repository, IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetLatestPosts(
        [FromQuery] PaginationParams paginationParams
    ) 
    {
        var posts = await repository.GetLatestPosts(paginationParams);

        Response.AddPaginationHeader(posts);

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id) 
    {
        Post? post = await repository.GetPost(id);

        if (post == null) return NotFound("Post was not found");
        
        return Ok(mapper.Map<PostDto>(post));
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsOfUser(
        string userName, 
        [FromQuery] PaginationParams paginationParams
    ) 
    {
        User? user = await userRepository.GetUserByUserNameAsNonDto(userName);
        if (user == null) return NotFound("User was not found");

        var posts = await repository.GetPostsOfUser(user, paginationParams);

        Response.AddPaginationHeader(posts);
        
        return Ok(posts);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] string text)
    {
        int userId = User.GetUserId();

        repository.CreatePost(
            new Post
            {
                UserId = userId,
                Text = text
            }
        );
        
        if (await repository.Complete()) return Ok();

        return BadRequest("Failed to create Post");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        int userId = User.GetUserId();

        Post? post = await repository.GetPost(id);
        if (post == null) return NotFound("Post was not found");

        if (post.UserId != userId) return BadRequest("Cannot delete someone elses post");

        repository.DeletePost(post);

        if (await repository.Complete()) return Ok();

        return BadRequest("Failed to delete Post");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] string text)
    {
        if (text == null || text == string.Empty) return BadRequest("You must provide a text");

        int userId = User.GetUserId();

        Post? post = await repository.GetPost(id);
        if (post == null) return NotFound("Post was not found");

        if (post.UserId != userId) return BadRequest("Cannot update someone elses post");

        post.Text = text;
        repository.UpdatePost(post);

        if (await repository.Complete()) return Ok();

        return BadRequest("Failed to update Post");
    }
}
