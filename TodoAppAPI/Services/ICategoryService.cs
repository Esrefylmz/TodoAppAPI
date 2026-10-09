using TodoAppAPI.DTOs.Category;
using TodoAppAPI.DTOs.Common;
using TodoAppAPI.DTOs.Todo;
using TodoAppCore.Models;
using TodoAppCore.Queries;

namespace TodoAppAPI.Services
{
    public interface ICategoryService
    {
        PagedResponseDto<CategoryResponseDto> GetList(CategoryQueryParameters queryParameters);
        CategoryResponseDto? GetById(int id);
        void Add(CreateCategoryDto category);
        void Update(int id, UpdateCategoryDto category);
        void Delete(int id);


    }
}
