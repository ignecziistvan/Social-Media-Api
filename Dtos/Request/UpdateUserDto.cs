namespace API.Dtos.Request;

public class UpdateUserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? OldPassword { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}