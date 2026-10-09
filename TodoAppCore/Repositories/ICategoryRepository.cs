using TodoAppCore.Models;
using TodoAppCore.Queries;

namespace TodoAppCore.Repositories
{
    public interface ICategoryRepository
    {
        PagedResult<Category> GetList(
            CategoryQueryParameters queryParameters,
            int userId);

        Category? GetById(int id, int userId);

        void Add(Category category);

        bool Update(Category category, int userId);

        bool Delete(int id, int userId);
    }
}