using System.ComponentModel.DataAnnotations;

namespace TodoAppApplication.DTOs.Category
{
    public class CreateCategoryDto
    {
        public string Title { get; set; } = string.Empty;
    }
}
