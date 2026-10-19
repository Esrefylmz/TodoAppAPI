using System.ComponentModel.DataAnnotations;

namespace TodoAppDTOs.Category
{
    public class CreateCategoryDto
    {
        public string Title { get; set; } = string.Empty;
    }
}
