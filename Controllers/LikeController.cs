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
        User? user = await userRepository.GetUserByUsername(username);
        if (user == null) return NotFound("User was not found by ID");

        return Ok(mapper.Map<List<LikeDto>>(await likeRepository.GetLikesOfUser(user)));
    }

    [Authorize]
    [HttpPost("post/{postId}")]
    public async Task<ActionResult<LikeDto>> LikePost(int postId)
    {
        User? user = await userRepository.GetUserById(User.GetUserId());
        if (user == null) return BadRequest("You are unauthorized");

        Post? post = await postRepository.GetPost(postId);
        if (post == null) return NotFound("Post was not found by ID");

        Like? existingLike = await likeRepository.GetSingleLikeByUserIdAndPostId(user.Id, postId);
        if (existingLike != null) 
            return BadRequest("You have already liked this Post");

        Like newLike = new() 
        {
            UserId = user.Id,
            PostId = postId,
            UserName = user.UserName!
        };

        likeRepository.LikePost(newLike);

        if (!await likeRepository.Complete()) return BadRequest("Could not like Post for some reason");

        return Ok(mapper.Map<LikeDto>(newLike));
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

        if (!await likeRepository.Complete()) return BadRequest("Could not like Post for some reason");

        return Ok();
    }
}
