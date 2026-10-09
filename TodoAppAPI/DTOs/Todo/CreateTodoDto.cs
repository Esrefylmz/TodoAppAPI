
namespace TodoAppAPI.DTOs.Todo
{
    public class CreateTodoDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }


        public int? CategoryId { get; set; }
    }
}
