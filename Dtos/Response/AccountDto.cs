namespace API.Dtos.Response;

public class AccountDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public string? Token { get; set; }
}
