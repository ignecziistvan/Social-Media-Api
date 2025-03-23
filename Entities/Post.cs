namespace API.Entities;

public class Post
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<Photo> Photos { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
    public List<Like> LikedByUsers { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.UtcNow;


    // Navigation properties
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}