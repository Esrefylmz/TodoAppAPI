using System.ComponentModel.DataAnnotations;

namespace TodoAppAPI.DTOs.Todo
{
    public class UpdateTodoDto
    {
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }


        public int? CategoryId { get; set; }
    }
}
