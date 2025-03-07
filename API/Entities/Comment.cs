using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities;

public class Comment
{
    public int Id { get; set; }
    public Post Post { get; set; } = null!;
    public int PostId { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }

    public string Text { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
