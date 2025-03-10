using API.Dtos.Response;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PostRepository(DataContext context, IMapper mapper) : IPostRepository
{
    public async Task<PaginatedList<PostDto>> GetLatestPosts(PaginationParams paginationParams)
    {
        IQueryable<PostDto> posts = context.Posts
            .OrderByDescending(p => p.Created)
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PaginatedList<PostDto>.CreateAsync(
            posts, paginationParams.PageNumber, paginationParams.PageSize
        );
    }

    public async Task<Post?> GetPost(int id)
    {
        return await context.Posts.FindAsync(id);
    }

    public async Task<PaginatedList<PostDto>> GetPostsOfUser(int userId, PaginationParams paginationParams)
    {
        var posts = context.Posts
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Created)
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await PaginatedList<PostDto>.CreateAsync(
            posts, paginationParams.PageNumber, paginationParams.PageSize
        );
    }

    public void CreatePost(Post post)
    {
        context.Posts.Add(post);
    }

    public void DeletePost(Post post)
    {
        context.Posts.Remove(post);
    }

    public void UpdatePost(Post post)
    {
        context.Entry(post).State = EntityState.Modified;
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
