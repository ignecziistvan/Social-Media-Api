namespace API.Dtos.Response;

public class PostDto
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required string UserName { get; set; }

    
    public required string Text { get; set; }
    public required List<CommentDto> Comments { get; set; }
    public required List<LikeDto> LikedByUsers { get; set; }
    public required DateTime Created { get; set; }
}
