using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.Response;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface ICommentRepository
{
    Task<CommentDto?> GetCommentById(int id);
    Task<List<CommentDto>> GetCommentsOfPost(int postId);
    Task<List<CommentDto>> GetCommentsOfUser(User user);
    void AddComment(Comment comment);
    Task<bool> Complete();
}
