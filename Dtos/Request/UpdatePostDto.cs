using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Request;

public class UpdatePostDto
{
    [Required]
    public string Text { get; set; } = string.Empty;
}