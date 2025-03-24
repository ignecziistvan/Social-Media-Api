namespace API.Entities;

public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}