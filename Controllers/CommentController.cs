using API.Dtos.Response;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CommentController(ICommentRepository commentRepository, 
    IPostRepository postRepository, IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentById(int id)
    {
        Comment? comment = await commentRepository.GetCommentById(id);

        if (comment == null) return NotFound("Comment was not found");

        return mapper.Map<CommentDto>(comment);
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfPost(
        int postId, 
        [FromQuery] PaginationParams paginationParams
    )
    {
        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found");

        return await commentRepository.GetCommentsOfPost(postId, paginationParams);
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfUser(
        string username, 
        [FromQuery] PaginationParams paginationParams
    )
    {
        User? user = await userRepository.GetUserByUsername(username);
        if (user == null) return NotFound("User was not found");

        return await commentRepository.GetCommentsOfUser(user, paginationParams);
    }

    [Authorize]
    [HttpPost("post/{postId}")]
    public async Task<ActionResult> AddCommentToPost(int postId, [FromBody] string text)
    {
        if (text == null || text == string.Empty) return BadRequest("Provide a text");
        
        int userId = User.GetUserId();

        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found");

        commentRepository.AddComment(
            new Comment 
            {
                UserId = userId,
                PostId = post.Id,
                Text = text
            }
        );
        
        if (await commentRepository.Complete()) return Ok();

        return BadRequest("Failed to create Comment");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        int userId = User.GetUserId();

        Comment? comment = await commentRepository.GetCommentById(id);
        if (comment == null) return NotFound("Comment was not found");

        if (comment.UserId != userId) return BadRequest("Cannot delete someone elses comment");

        commentRepository.DeleteComment(comment);
        
        if (await commentRepository.Complete()) return Ok();

        return BadRequest("Failed to delete Comment");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] string text)
    {
        if (text == null || text == string.Empty) return BadRequest("You must provide a text");

        int userId = User.GetUserId();

        Comment? comment = await commentRepository.GetCommentById(id);
        if (comment == null) return NotFound("Comment was not found");

        if (comment.UserId != userId) return BadRequest("Cannot update someone elses comment");

        comment.Text = text;
        commentRepository.UpdateComment(comment);
        
        if (await commentRepository.Complete()) return Ok();

        return BadRequest("Failed to update Comment");
    }
}