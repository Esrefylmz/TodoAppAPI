using TodoAppCore.Models;

namespace TodoAppCore.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetQueryable();

        Category? GetById(int id, int userId);

        void Add(Category category);

        bool Update(Category category, int userId);

        bool Delete(int id, int userId);
    }
}