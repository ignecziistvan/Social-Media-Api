namespace API.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;


    // Navigation properties
    public int PostId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;
}
