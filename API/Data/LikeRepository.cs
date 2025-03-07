using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikeRepository(DataContext context) : ILikeRepository
{
    public async Task<Like?> GetLikeById(int id)
    {
        return await context.PostLikes
            .Where(l => l.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Like>> GetLikesOfPostById(int postId)
    {
        return await context.PostLikes
            .Where(l => l.PostId == postId)
            .ToListAsync();
    }

    public async Task<List<Like>> GetLikesOfUser(User user)
    {
        return await context.PostLikes
            .Where(l => l.User == user)
            .ToListAsync();
    }

    public async Task<Like?> GetSingleLikeByUserIdAndPostId(int userId, int postId)
    {
        return await context.PostLikes
            .Where(l => l.UserId == userId && l.PostId == postId)
            .SingleOrDefaultAsync();
    }

    public void LikePost(Like like)
    {
        context.PostLikes.Add(like);
    }

    public void UnlikePost(Like like)
    {
        context.PostLikes.Remove(like);
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
