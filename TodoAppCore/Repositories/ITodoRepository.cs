using TodoAppCore.Models;
using TodoAppCore.Queries;

namespace TodoAppCore.Repositories
{
    public interface ITodoRepository
    {
        //Add getList Update Delete
        PagedResult<Todo> GetList(TodoQueryParameters queryParameters, int userId);
        void Add(Todo todo);
        bool Update(Todo todo, int userId);
        bool Delete(int id, int userId);

    }
}
