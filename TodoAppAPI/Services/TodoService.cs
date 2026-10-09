using AutoMapper;
using TodoAppAPI.DTOs.Common;
using TodoAppAPI.DTOs.Todo;
using TodoAppAPI.ExceptionHandling.Exceptions;
using TodoAppCore.Models;
using TodoAppCore.Queries;
using TodoAppCore.Repositories;

namespace TodoAppAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public TodoService(
            ITodoRepository todoRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _todoRepository = todoRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public PagedResponseDto<TodoResponseDto> GetList(TodoQueryParameters queryParameters)
        {

            /*
            var response = todos.Select(todo => new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt
            });
            */
            var result = _todoRepository.GetList(queryParameters, _currentUserService.UserId);

            var items = _mapper.Map<IEnumerable<TodoResponseDto>>(result.Items);

            var totalPages = (int)Math.Ceiling(result.TotalCount / (double)queryParameters.PageSize);

            return new PagedResponseDto<TodoResponseDto>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = totalPages
            };
        }

        public void Add(CreateTodoDto todo)
        {
            /*
            var newTodo = new Todo
            {
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            */
            if (todo.CategoryId.HasValue)
            {
                var category = _categoryRepository.GetById(todo.CategoryId.Value, _currentUserService.UserId);

                if (category == null)
                {
                    throw new NotFoundException(
                        $"Category with id {todo.CategoryId.Value} was not found.");
                }
            }


            var newTodo = _mapper.Map<Todo>(todo);
            newTodo.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            newTodo.UserId = _currentUserService.UserId;

            _todoRepository.Add(newTodo);
        }

        public void Update(int id, UpdateTodoDto todo)
        {
            /*
            var updatedTodo = new Todo
            {
                Id = id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted
            };
            */
            if (todo.CategoryId.HasValue)
            {
                var category = _categoryRepository.GetById(todo.CategoryId.Value, _currentUserService.UserId);

                if (category == null)
                {
                    throw new NotFoundException(
                        $"Category with id {todo.CategoryId.Value} was not found.");
                }
            }


            var updatedTodo = _mapper.Map<Todo>(todo);
            updatedTodo.Id = id;

            var result = _todoRepository.Update(updatedTodo, _currentUserService.UserId);

            if (!result)
            {
                throw new NotFoundException($"Todo with id {id} was not found.");
            }

        }

        public void Delete(int id)
        {
            var result = _todoRepository.Delete(id, _currentUserService.UserId);

            if (!result)
            {
                throw new NotFoundException($"Todo with id {id} was not found.");
            }
        }




    }
}