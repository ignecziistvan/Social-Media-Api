using API.Dtos.Response;
using API.Entities;

namespace API.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<PostDto>> GetLatestPosts(int? count);
    Task<IEnumerable<PostDto>> GetPostsOfUser(User user);
    Task<PostDto?> GetPost(int id);
    void CreatePost(Post post);
    Task<bool> Complete();
}
