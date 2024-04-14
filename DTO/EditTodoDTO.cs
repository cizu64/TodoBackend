public record EditTodoDTO
{
    public bool IsComplete { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
}