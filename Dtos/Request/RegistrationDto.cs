using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Request;

public class RegistrationDto
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(32, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string? Firstname { get; set; }

    [Required]
    public string? Lastname { get; set; }

    [Required]
    public string? DateOfBirth { get; set; }
}
