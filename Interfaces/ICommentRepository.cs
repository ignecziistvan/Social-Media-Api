using API.Dtos.Response;
using API.Entities;

namespace API.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetCommentById(int id);
    Task<List<CommentDto>> GetCommentsOfPost(int postId);
    Task<List<CommentDto>> GetCommentsOfUser(User user);
    void AddComment(Comment comment);
    void DeleteComment(Comment comment);
    void UpdateComment(Comment comment);
    Task<bool> Complete();
}
