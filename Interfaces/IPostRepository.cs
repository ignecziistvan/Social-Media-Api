using API.Dtos.Response;
using API.Entities;

namespace API.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<PostDto>> GetLatestPosts(int? count);
    Task<IEnumerable<PostDto>> GetPostsOfUser(User user);
    Task<Post?> GetPost(int id);
    void CreatePost(Post post);
    void DeletePost(Post post);
    void UpdatePost(Post post);
    Task<bool> Complete();
}
