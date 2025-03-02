using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class User : IdentityUser<int>
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public DateOnly DateOfBirth { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public List<Post> Posts { get; set; } = [];
}
