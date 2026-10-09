using System.ComponentModel.DataAnnotations;

namespace TodoAppCore.Models
{
    public class Todo
    {
        //id, Title, IsCompleted, CreatedAt
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        [DataType(DataType.Date)]
        public DateOnly CreatedAt { get; set; }



        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

    }

}
