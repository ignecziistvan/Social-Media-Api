using API.Dtos.Response;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PostRepository(DataContext context, IMapper mapper) : IPostRepository
{
    private readonly int DefaultCount = 20;
    
    public async Task<IEnumerable<PostDto>> GetLatestPosts(int? count)
    {
        return await context.Posts
            .Take(count ?? DefaultCount)
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Post?> GetPost(int id)
    {
        return await context.Posts.FindAsync(id);
    }

    public async Task<IEnumerable<PostDto>> GetPostsOfUser(User user)
    {
        return await context.Posts
            .Where(p => p.User == user)
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .ToListAsync();
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
