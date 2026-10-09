using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppAPI.DTOs.Todo;
using TodoAppAPI.Services;
using TodoAppCore.Queries;

namespace TodoAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        //public static List<Todo> TodoList { get; set; } = new();
        private readonly ITodoService _todoService;
        private readonly IValidator<CreateTodoDto> _createTodoValidator;
        private readonly IValidator<UpdateTodoDto> _updateTodoValidator;
        private readonly IValidator<TodoQueryParameters> _queryValidator;

        public TodoController(
            ITodoService todoService,
            IValidator<CreateTodoDto> createTodoValidator,
            IValidator<UpdateTodoDto> updateTodoValidator, 
            IValidator<TodoQueryParameters> queryValidator)
        {
            _todoService = todoService;
            _createTodoValidator = createTodoValidator;
            _updateTodoValidator = updateTodoValidator;
            _queryValidator = queryValidator;
        }

        [HttpPost]
        public IActionResult Add(CreateTodoDto todo)
        {
            _createTodoValidator.ValidateAndThrow(todo);

            _todoService.Add(todo);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] TodoQueryParameters queryParameters)
        {
            _queryValidator.ValidateAndThrow(queryParameters);

            return Ok(_todoService.GetList(queryParameters));
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateTodoDto todo)
        {
            _updateTodoValidator.ValidateAndThrow(todo);

            _todoService.Update(id, todo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _todoService.Delete(id);

            return NoContent();
        }

    }
}
