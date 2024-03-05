using System.ComponentModel.DataAnnotations;

public record CompleteTodoDTO
{
   [Required]
   public bool IsComplete{get;set;}
}