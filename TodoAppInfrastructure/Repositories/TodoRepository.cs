using Microsoft.EntityFrameworkCore;
using TodoAppCore.Models;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;

namespace TodoAppInfrastructure.Repositories
{
    public class TodoRepository: BaseRepository<Todo>, ITodoRepository
    {
        public TodoRepository(TodoContext dbContext)
            : base(dbContext)
        {
        }

        public bool Delete(int id, int userId)
        {
            var existingTodo = _dbSet
                .FirstOrDefault(t =>
                    t.Id == id &&
                    t.UserId == userId);

            if (existingTodo == null)
            {
                return false;
            }

            Remove(existingTodo);
            SaveChanges();

            return true;
        }

        public IQueryable<Todo> GetQueryable()
        {
            return _dbSet
                .Include(t => t.Category)
                .AsQueryable();
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