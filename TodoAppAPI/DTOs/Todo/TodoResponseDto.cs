using TodoAppAPI.DTOs.Category;

namespace TodoAppAPI.DTOs.Todo
{
    public class TodoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateOnly CreatedAt { get; set; }


        public int? CategoryId { get; set; }
        public CategoryResponseDto? Category { get; set; }
    }
}
