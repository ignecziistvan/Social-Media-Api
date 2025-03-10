namespace API.Entities;

public class Like
{
    public int Id { get; set; }
    public required string UserName { get; set; }


    // Navigation properties
    public required int UserId { get; set; }
    public required int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}
