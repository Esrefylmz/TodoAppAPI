using System.ComponentModel.DataAnnotations;

namespace TodoAppAPI.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public string Title { get; set; } = string.Empty;
    }
}
