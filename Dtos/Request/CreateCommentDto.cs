using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Request;

public class CreateCommentDto
{
    [Required]
    public string Text { get; set; } = string.Empty;
}