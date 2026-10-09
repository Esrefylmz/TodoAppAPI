using Microsoft.EntityFrameworkCore;
using TodoAppCore.Models;
using TodoAppCore.Queries;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;

namespace TodoAppInfrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _dbContext;

        public TodoRepository(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Todo todo)
        {
            _dbContext.Todos.Add(todo);
            _dbContext.SaveChanges();
        }

        public bool Delete(int id, int userId)
        {
            var existingTodo = _dbContext.Todos
                .FirstOrDefault(t =>
                    t.Id == id &&
                    t.UserId == userId);

            if (existingTodo == null)
            {
                return false;
            }

            _dbContext.Todos.Remove(existingTodo);
            _dbContext.SaveChanges();

            return true;
        }

        public Todo? GetById(int id)
        {
            return _dbContext.Todos
                .Include(t => t.Category)
                .FirstOrDefault(t => t.Id == id);
        }

        public PagedResult<Todo> GetList(TodoQueryParameters queryParameters, int userId)
        {
            var query = _dbContext.Todos
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (queryParameters.IsCompleted.HasValue)
            {
                query = query.Where(t =>
                    t.IsCompleted == queryParameters.IsCompleted.Value);
            }

            if (queryParameters.CategoryId.HasValue)
            {
                query = query.Where(t =>
                    t.CategoryId == queryParameters.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                query = query.Where(t =>
                    t.Title.Contains(queryParameters.Search));
            }

            if (queryParameters.SortBy.HasValue)
            {
                var isDescending =
                    queryParameters.SortDirection == SortDirection.Desc;

                query = queryParameters.SortBy.Value switch
                {
                    TodoSortBy.Title => isDescending
                        ? query.OrderByDescending(t => t.Title)
                        : query.OrderBy(t => t.Title),

                    TodoSortBy.CreatedAt => isDescending
                        ? query.OrderByDescending(t => t.CreatedAt)
                        : query.OrderBy(t => t.CreatedAt),

                    TodoSortBy.Id => isDescending
                        ? query.OrderByDescending(t => t.Id)
                        : query.OrderBy(t => t.Id),

                    _ => query.OrderBy(t => t.Id)
                };
            }
            else
            {
                query = query.OrderBy(t => t.Id);
            }

            var totalCount = query.Count();
            query = query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize) //skip 0 take 10
                .Take(queryParameters.PageSize);

            return new PagedResult<Todo>
            {
                Items = query.ToList(),
                TotalCount = totalCount
            };

        }

        public bool Update(Todo todo, int userId)
        {
            var existingTodo = _dbContext.Todos
                .FirstOrDefault(t =>
                    t.Id == todo.Id &&
                    t.UserId == userId);

            if (existingTodo == null)
            {
                return false;
            }

            existingTodo.Title = todo.Title;
            existingTodo.IsCompleted = todo.IsCompleted;
            existingTodo.CategoryId = todo.CategoryId;

            _dbContext.SaveChanges();

            return true;
        }
    }
}