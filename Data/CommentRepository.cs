using API.Dtos.Response;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CommentRepository(DataContext context, IMapper mapper) : ICommentRepository
{
    public void AddComment(Comment comment)
    {
        context.Comments.Add(comment);
    }

    public void DeleteComment(Comment comment)
    {
        context.Comments.Remove(comment);
    }

    public void UpdateComment(Comment comment)
    {
        context.Entry(comment).State = EntityState.Modified;
    }

    public async Task<Comment?> GetCommentById(int id)
    {
        return await context.Comments
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<CommentDto>> GetCommentsOfPost(int postId)
    {
        return await context.Comments
            .Where(c => c.PostId == postId)
            .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<CommentDto>> GetCommentsOfUser(User user)
    {
        return await context.Comments
            .Where(c => c.User == user)
            .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
