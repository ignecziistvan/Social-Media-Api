using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Request;

public class UpdateCommentDto
{
    [Required]
    public string Text { get; set; } = string.Empty;
}