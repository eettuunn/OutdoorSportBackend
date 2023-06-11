using System.ComponentModel.DataAnnotations;

namespace Backend.Common.Dtos;

public class CreateCommentDto
{
    [Required]
    public string text { get; set; }
}