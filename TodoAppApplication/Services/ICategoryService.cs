using TodoAppDTOs.Category;
using TodoAppDTOs.Common;
using TodoAppDTOs.Todo;
using TodoAppCore.Models;
using TodoAppCore.Queries;

namespace TodoAppApplication.Services
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
