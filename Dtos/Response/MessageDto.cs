namespace API.Dtos.Response;

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public required string SenderUsername { get; set; }
    public PhotoDto? SenderMainPhoto { get; set; }
    public int ReceiverId { get; set; }
    public required string ReceiverUsername { get; set; }
    public PhotoDto? ReceiverMainPhoto { get; set; }
    public required string Text { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }
}