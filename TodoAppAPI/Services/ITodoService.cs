using TodoAppAPI.DTOs.Common;
using TodoAppAPI.DTOs.Todo;
using TodoAppCore.Models;
using TodoAppCore.Queries;

namespace TodoAppAPI.Services
{
    public interface ITodoService
    {
        PagedResponseDto<TodoResponseDto> GetList(TodoQueryParameters queryParameters);
        void Add(CreateTodoDto todo);
        void Update(int id, UpdateTodoDto todo);
        void Delete(int id);
    }
}
