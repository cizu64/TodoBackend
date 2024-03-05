using System.ComponentModel.DataAnnotations;

public record UserDTO
{
   [Required]
   [EmailAddress]
   public string Email { get; set; }
   [Required]
   [MinLength(7)]
   public string Password { get; set; }
   [Required]
   public string Firstname{get;set;}
   [Required]
   public string LastName{get;set;}    
}