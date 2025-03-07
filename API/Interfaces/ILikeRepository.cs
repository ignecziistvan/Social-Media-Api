using API.Entities;

namespace API.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLikeById(int id);

    Task<List<Like>> GetLikesOfPostById(int postId);
    
    Task<List<Like>> GetLikesOfUser(User user);

    void LikePost(Like like);
    void UnlikePost(Like like);

    Task<Like?> GetSingleLikeByUserIdAndPostId(int userId, int postId);

    Task<bool> Complete();
}
