using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Request;

public class LoginDto
{
    [Required]
    public string UserNameOrEmail { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}
