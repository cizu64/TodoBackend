using System.ComponentModel.DataAnnotations;

public record LoginDTO
{
   [Required]
   [EmailAddress]
   public string Email { get; set; }
   [Required]
   public string Password { get; set; }
}