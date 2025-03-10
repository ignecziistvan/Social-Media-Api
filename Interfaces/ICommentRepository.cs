using API.Dtos.Response;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetCommentById(int id);
    Task<PaginatedList<CommentDto>> GetCommentsOfPost(int postId, PaginationParams paginationParams);
    Task<PaginatedList<CommentDto>> GetCommentsOfUser(User user, PaginationParams paginationParams);
    void AddComment(Comment comment);
    void DeleteComment(Comment comment);
    void UpdateComment(Comment comment);
    Task<bool> Complete();
}
