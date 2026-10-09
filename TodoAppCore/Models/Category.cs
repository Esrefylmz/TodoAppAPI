namespace TodoAppCore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;


        public ICollection<Todo> Todos { get; set; } = new List<Todo>();

        public int? UserId { get; set; }
        public User? User { get; set; }

    }
}