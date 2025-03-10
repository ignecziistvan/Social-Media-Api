using API.Dtos.Response;
using API.Entities;
using API.Helpers;
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
        return await context.Comments.FindAsync(id);
    }

    public async Task<PaginatedList<CommentDto>> GetCommentsOfPost(int postId, PaginationParams paginationParams)
    {
        var comments = context.Comments
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.Created)
            .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PaginatedList<CommentDto>.CreateAsync(
            comments, paginationParams.PageNumber, paginationParams.PageSize
        );
    }

    public async Task<PaginatedList<CommentDto>> GetCommentsOfUser(User user, PaginationParams paginationParams)
    {
        var comments = context.Comments
            .Where(c => c.User == user)
            .OrderByDescending(c => c.Created)
            .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
            .AsQueryable();;

        return await PaginatedList<CommentDto>.CreateAsync(
            comments, paginationParams.PageNumber, paginationParams.PageSize
        );
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
