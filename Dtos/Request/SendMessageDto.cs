namespace API.Dtos.Request;

public class SendMessageDto
{
    public required string ReceiverUsername { get; set; }
    public required string Text { get; set; }
}
