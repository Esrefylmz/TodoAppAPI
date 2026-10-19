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

        public PagedResult<Category> GetList(CategoryQueryParameters queryParameters, int userId)
        {
            var query = _dbContext.Categories
                .Where(c => c.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                query = query.Where(c =>
                    c.Title.Contains(queryParameters.Search));
            }

            if (queryParameters.SortBy.HasValue)
            {
                var isDescending =
                    queryParameters.SortDirection == SortDirection.Desc;

                query = queryParameters.SortBy.Value switch
                {
                    CategorySortBy.Title => isDescending
                        ? query.OrderByDescending(c => c.Title)
                        : query.OrderBy(c => c.Title),

                    CategorySortBy.Id => isDescending
                        ? query.OrderByDescending(c => c.Id)
                        : query.OrderBy(c => c.Id),

                    _ => query.OrderBy(c => c.Id)
                };
            }
            else
            {
                query = query.OrderBy(c => c.Id);
            }

            var totalCount = query.Count();

            query = query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize);

            return new PagedResult<Category>
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };
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