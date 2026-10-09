using System.ComponentModel.DataAnnotations;

namespace TodoAppAPI.DTOs.Category
{
    public class CreateCategoryDto
    {
        public string Title { get; set; } = string.Empty;
    }
}
