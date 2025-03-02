using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Response;

public class UserDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
}
