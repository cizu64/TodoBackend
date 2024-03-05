namespace TODOBACKEND.Entities;
public class Todo
{
   public int Id { get; set; }
   public int UserId{get;set;}
   public string Title { get; set; }
   public string Description { get; set; }
 
   public bool IsComplete{get;set;}=false;
   public DateTime DateCreated{get;set;}=DateTime.UtcNow;
   public User User{get;set;}
}