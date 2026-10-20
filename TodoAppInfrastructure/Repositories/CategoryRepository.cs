using TodoAppCore.Models;
using TodoAppCore.Queries;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;

namespace TodoAppInfrastructure.Repositories
{
    public class CategoryRepository
        : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(TodoContext dbContext)
            : base(dbContext)
        {
        }


        public bool Delete(int id, int userId)
        {
            var existingCategory = _dbSet
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UserId == userId);

            if (existingCategory == null)
            {
                return false;
            }

            Remove(existingCategory);
            SaveChanges();

            return true;
        }

        public Category? GetById(int id, int userId)
        {
            return _dbContext.Categories
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UserId == userId);
        }

        public IQueryable<Category> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public bool Update(Category category, int userId)
        {
            var existingCategory = _dbContext.Categories
                .FirstOrDefault(c =>
                    c.Id == category.Id &&
                    c.UserId == userId);

            if (existingCategory == null)
            {
                return false;
            }

            existingCategory.Title = category.Title;

            _dbContext.SaveChanges();

            return true;
        }
    }
}