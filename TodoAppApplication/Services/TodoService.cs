using AutoMapper;
using TodoAppDTOs.Common;
using TodoAppDTOs.Todo;
using TodoAppApplication.Exceptions;
using TodoAppCore.Models;
using TodoAppCore.Queries;
using TodoAppCore.Repositories;
using TodoAppApplication.Abstractions;

namespace TodoAppApplication.Services
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

        public PagedResponseDto<TodoResponseDto> GetList(
            TodoQueryParameters queryParameters)
        {
            var userId = _currentUserService.UserId;

            var query = _todoRepository
                .GetQueryable()
                .Where(t => t.UserId == userId);


            // Filtering
            if (queryParameters.IsCompleted.HasValue)
            {
                query = query.Where(
                    t => t.IsCompleted == queryParameters.IsCompleted.Value);
            }

            if (queryParameters.CategoryId.HasValue)
            {
                query = query.Where(
                    t => t.CategoryId == queryParameters.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                query = query.Where(
                    t => t.Title.Contains(queryParameters.Search));
            }


            // Sorting
            query = queryParameters.SortBy switch
            {
                TodoSortBy.Title =>
                    queryParameters.SortDirection == SortDirection.Desc
                        ? query.OrderByDescending(t => t.Title)
                        : query.OrderBy(t => t.Title),

                TodoSortBy.CreatedAt =>
                    queryParameters.SortDirection == SortDirection.Desc
                        ? query.OrderByDescending(t => t.CreatedAt)
                        : query.OrderBy(t => t.CreatedAt),

                TodoSortBy.Id =>
                    queryParameters.SortDirection == SortDirection.Desc
                        ? query.OrderByDescending(t => t.Id)
                        : query.OrderBy(t => t.Id),

                _ => query.OrderBy(t => t.Id)
            };


            // Total count before pagination
            var totalCount = query.Count();


            // Pagination
            var todos = query
                .Skip(
                    (queryParameters.PageNumber - 1)
                    * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToList();


            // Mapping
            var items = _mapper.Map<List<TodoResponseDto>>(todos);


            return new PagedResponseDto<TodoResponseDto>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)queryParameters.PageSize)
            };
        }

        public void Add(CreateTodoDto todo)
        {
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