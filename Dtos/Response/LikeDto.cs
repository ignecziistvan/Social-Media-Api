namespace API.Dtos.Response;

public class LikeDto
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
    public required string UserName { get; set; }
    public PhotoDto? UserMainPhoto { get; set; }
}
