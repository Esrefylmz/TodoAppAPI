using TodoAppCore.Models;

namespace TodoAppCore.Repositories
{
    public interface ITodoRepository
    {
        IQueryable<Todo> GetQueryable();
        void Add(Todo todo);
        bool Update(Todo todo, int userId);
        bool Delete(int id, int userId);

    }
}
