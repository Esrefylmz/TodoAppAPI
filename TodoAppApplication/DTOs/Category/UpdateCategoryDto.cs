using System.ComponentModel.DataAnnotations;

namespace TodoAppApplication.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public string Title { get; set; } = string.Empty;
    }
}
