using API.Dtos.Response;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikeController(ILikeRepository likeRepository, IPostRepository postRepository, 
    IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<LikeDto>> GetLikeById(int id)
    {
        Like? like = await likeRepository.GetLikeById(id);
        if (like == null) return NotFound("Like was not found by ID");

        return Ok(mapper.Map<LikeDto>(like));
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<List<LikeDto>>> GetLikesOfPost(int postId)
    {
        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found by ID");

        return Ok(mapper.Map<List<LikeDto>>(await likeRepository.GetLikesOfPostById(postId)));
    }

    [HttpGet("user/{username}")]
    public async Task<ActionResult<List<LikeDto>>> GetLikesOfUser(string username)
    {
        User? user = await userRepository.GetUserByUserNameAsNonDto(username);
        if (user == null) return NotFound("User was not found by ID");

        return Ok(mapper.Map<List<LikeDto>>(await likeRepository.GetLikesOfUser(user)));
    }

    [Authorize]
    [HttpPost("post/{postId}")]
    public async Task<ActionResult<bool>> LikePost(int postId)
    {
        int userId = User.GetUserId();
        string username = User.GetUsername();

        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found by ID");

        Like? like = await likeRepository.GetSingleLikeByUserIdAndPostId(userId, postId);
        if (like != null) 
            return BadRequest("You have already liked this Post");

        likeRepository.LikePost(
            new Like
            {
                UserId = userId,
                PostId = postId,
                UserName = username
            }
        );

        if (await likeRepository.Complete()) return Ok();

        return BadRequest("Could not like Post for some reason");
    }

    [Authorize]
    [HttpDelete("post/{postId}")]
    public async Task<ActionResult<bool>> UnlikePost(int postId)
    {
        int userId = User.GetUserId();

        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found by ID");

        Like? like = await likeRepository.GetSingleLikeByUserIdAndPostId(userId, postId);
        if (like == null) 
            return BadRequest("You have not liked this Post yet");

        likeRepository.UnlikePost(like);

        if (await likeRepository.Complete()) return Ok();

        return BadRequest("Could not like Post for some reason");
    }
}
