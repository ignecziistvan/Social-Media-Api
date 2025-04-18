namespace API.Dtos.Response;

public class CommentDto
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required string UserName { get; set; }
    public required int PostId { get; set; }

    
    public required string Text { get; set; }
    public required DateTime Created { get; set; }
}