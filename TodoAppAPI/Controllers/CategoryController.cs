using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppAPI.DTOs.Category;
using TodoAppAPI.Services;
using TodoAppCore.Queries;

namespace TodoAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CreateCategoryDto> _createCategoryValidator;
        private readonly IValidator<UpdateCategoryDto> _updateCategoryValidator;
        private readonly IValidator<CategoryQueryParameters> _queryValidator;

        public CategoryController(
            ICategoryService categoryService,
            IValidator<CreateCategoryDto> createCategoryValidator,
            IValidator<UpdateCategoryDto> updateCategoryValidator,
            IValidator<CategoryQueryParameters> queryValidator)
        {
            _categoryService = categoryService;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
            _queryValidator = queryValidator;
        }

        [HttpPost]
        public IActionResult Add(CreateCategoryDto category)
        {
            _createCategoryValidator.ValidateAndThrow(category);

            _categoryService.Add(category);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult GetList(
            [FromQuery] CategoryQueryParameters queryParameters)
        {
            _queryValidator.ValidateAndThrow(queryParameters);

            return Ok(_categoryService.GetList(queryParameters));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCategoryDto category)
        {
            _updateCategoryValidator.ValidateAndThrow(category);

            _categoryService.Update(id, category);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);

            return NoContent();
        }


    }
}
