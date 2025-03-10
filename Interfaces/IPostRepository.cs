using API.Dtos.Response;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IPostRepository
{
    Task<PaginatedList<PostDto>> GetLatestPosts(PaginationParams paginationParams);
    Task<PaginatedList<PostDto>> GetPostsOfUser(int userId, PaginationParams paginationParams);
    Task<Post?> GetPost(int id);
    void CreatePost(Post post);
    void DeletePost(Post post);
    void UpdatePost(Post post);
    Task<bool> Complete();
}
