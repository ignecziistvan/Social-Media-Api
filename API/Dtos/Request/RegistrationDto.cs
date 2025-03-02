using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
