using API.Dtos.Request;
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

        return Ok(mapper.Map<CommentDto>(comment));
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfPost(
        int postId, 
        [FromQuery] PaginationParams paginationParams
    )
    {
        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found");

        return Ok(await commentRepository.GetCommentsOfPost(postId, paginationParams));
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfUser(
        string username, 
        [FromQuery] PaginationParams paginationParams
    )
    {
        User? user = await userRepository.GetUserByUsername(username);
        if (user == null) return NotFound("User was not found");

        return Ok(await commentRepository.GetCommentsOfUser(user, paginationParams));
    }

    [Authorize]
    [HttpPost("post/{postId}")]
    public async Task<ActionResult<CommentDto>> AddCommentToPost(int postId, CreateCommentDto dto)
    {
        if (dto.Text == null || dto.Text == string.Empty) return BadRequest("Provide a text");
        
        User? user = await userRepository.GetUserById(User.GetUserId());
        if (user == null) return BadRequest("You are unauthenticated");

        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found");

        Comment comment = new() 
        {
            User = user,
            PostId = post.Id,
            Text = dto.Text
        };
        commentRepository.AddComment(comment);
        
        if (!await commentRepository.Complete()) return BadRequest("Failed to create Comment");

        CommentDto commentDto = mapper.Map<CommentDto>(comment);
        commentDto.UserName = User.GetUsername();

        return Ok(commentDto);
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
        
        if (!await commentRepository.Complete()) return BadRequest("Failed to delete Comment");

        return Ok();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, UpdateCommentDto dto)
    {
        if (dto.Text == null || dto.Text == string.Empty) return BadRequest("You must provide a text");

        int userId = User.GetUserId();

        Comment? comment = await commentRepository.GetCommentById(id);
        if (comment == null) return NotFound("Comment was not found");

        if (comment.UserId != userId) return BadRequest("Cannot update someone elses comment");

        comment.Text = dto.Text;
        commentRepository.UpdateComment(comment);
        
        if (!await commentRepository.Complete()) return BadRequest("Failed to update Comment");

        return Ok(mapper.Map<CommentDto>(comment));
    }
}