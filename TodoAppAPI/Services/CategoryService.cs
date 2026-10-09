using AutoMapper;
using TodoAppAPI.DTOs.Category;
using TodoAppAPI.DTOs.Common;
using TodoAppAPI.ExceptionHandling.Exceptions;
using TodoAppCore.Models;
using TodoAppCore.Queries;
using TodoAppCore.Repositories;



namespace TodoAppAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public void Add(CreateCategoryDto category)
        {
            /*
            var newCategory = new Category
            {
                Title = category.Title,

            };
            */
            var newCategory = _mapper.Map<Category>(category);

            newCategory.UserId = _currentUserService.UserId;

            _categoryRepository.Add(newCategory);
        }

        public void Delete(int id)
        {
            var result = _categoryRepository.Delete(id, _currentUserService.UserId);

            if (!result)
            {
                throw new NotFoundException(
                    $"Category with id {id} was not found.");
            }
        }

        public CategoryResponseDto GetById(int id)
        {
            //return _categoryRepository.GetById(id);

            var category = _categoryRepository.GetById(id, _currentUserService.UserId);

            if (category == null)
            {
                throw new NotFoundException(
                    $"Category with id {id} was not found.");
            }

            /*
            var response = new CategoryResponseDto
            {
                Id = category.Id,
                Title = category.Title,
            };
            */
            var response = _mapper.Map<CategoryResponseDto>(category);

            return response;
        }

        public PagedResponseDto<CategoryResponseDto> GetList(CategoryQueryParameters queryParameters)
        {
            var result = _categoryRepository.GetList(queryParameters, _currentUserService.UserId);

            var items =
                _mapper.Map<IEnumerable<CategoryResponseDto>>(result.Items);

            var totalPages = (int)Math.Ceiling(
                result.TotalCount / (double)queryParameters.PageSize);

            return new PagedResponseDto<CategoryResponseDto>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = totalPages
            };
        }

        public void Update(int id, UpdateCategoryDto category)
        {
            /*
            var updatedCategory = new Category
            {
                Id = id,
                Title = category.Title,
            };
            */
            var updatedCategory = _mapper.Map<Category>(category);
            updatedCategory.Id = id;

            var result = _categoryRepository.Update(updatedCategory, _currentUserService.UserId);

            if (!result)
            {
                throw new NotFoundException(
                    $"Category with id {id} was not found.");
            }
        }
    }
}
