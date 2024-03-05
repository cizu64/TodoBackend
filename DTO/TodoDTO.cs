using System.ComponentModel.DataAnnotations;

public record TodoDTO
{
   [Required]
   public string Title { get; set; }
   [Required]
   [MaxLength(300)]
   public string Description { get; set; }
}