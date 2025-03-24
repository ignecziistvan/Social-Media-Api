namespace API.Entities;

public class ProfilePhoto
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; } = false;
    public string? PublicId { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}