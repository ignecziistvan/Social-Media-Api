using API.Dtos.Response;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CommentController(ICommentRepository commentRepository, 
    IPostRepository postRepository, IUserRepository userRepository, 
    IMapper mapper) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentById(int id)
    {
        CommentDto? comment = await commentRepository.GetCommentById(id);

        if (comment == null) return NotFound("Comment was nor found");

        return mapper.Map<CommentDto>(comment);
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfPost(int postId)
    {
        PostDto? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found");

        return await commentRepository.GetCommentsOfPost(postId);
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfUser(string username)
    {
        User? user = await userRepository.GetUserByUserNameAsNonDto(username);
        if (user == null) return NotFound("User was not found");

        return await commentRepository.GetCommentsOfUser(user);
    }

    [Authorize]
    [HttpPost("post/{postId}")]
    public async Task<ActionResult<PostDto>> AddCommentToPost([FromRoute] int postId, [FromBody] string text)
    {
        if (text == null || text == string.Empty) return BadRequest("Provide a text");
        
        string? userName = User.Identity?.Name;
        if (userName == null) return Forbid("No identity");

        User? user = await userRepository.GetUserByUserNameAsNonDto(userName);
        if (user == null) return NotFound("User was not found");

        PostDto? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found");

        commentRepository.AddComment(
            new Comment 
            {
                UserId = user.Id,
                PostId = post.Id,
                Text = text
            }
        );
        
        if (await commentRepository.Complete()) return Ok();

        return BadRequest("Failed to create Post");
    }
}