using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Request;

public class CreatePostDto
{
    [Required]
    public string Text { get; set; } = string.Empty;
}