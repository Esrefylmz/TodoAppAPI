using AutoMapper;
using TodoAppDTOs.Category;
using TodoAppDTOs.Common;
using TodoAppApplication.Exceptions;
using TodoAppCore.Models;
using TodoAppCore.Queries;
using TodoAppCore.Repositories;
using TodoAppApplication.Abstractions;



namespace TodoAppApplication.Services
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

            var category = _categoryRepository.GetById(id, _currentUserService.UserId);

            if (category == null)
            {
                throw new NotFoundException(
                    $"Category with id {id} was not found.");
            }

            var response = _mapper.Map<CategoryResponseDto>(category);

            return response;
        }

        public PagedResponseDto<CategoryResponseDto> GetList(
            CategoryQueryParameters queryParameters)
        {
            var userId = _currentUserService.UserId;

            var query = _categoryRepository
                .GetQueryable()
                .Where(c => c.UserId == userId);


            // Filtering
            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                query = query.Where(
                    c => c.Title.Contains(queryParameters.Search));
            }


            // Sorting
            query = queryParameters.SortBy switch
            {
                CategorySortBy.Title =>
                    queryParameters.SortDirection == SortDirection.Desc
                        ? query.OrderByDescending(c => c.Title)
                        : query.OrderBy(c => c.Title),

                CategorySortBy.Id =>
                    queryParameters.SortDirection == SortDirection.Desc
                        ? query.OrderByDescending(c => c.Id)
                        : query.OrderBy(c => c.Id),

                _ => query.OrderBy(c => c.Id)
            };


            // Total count
            var totalCount = query.Count();


            // Pagination
            var categories = query
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToList();


            // Mapping
            var items =
                _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)queryParameters.PageSize);

            return new PagedResponseDto<CategoryResponseDto>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public void Update(int id, UpdateCategoryDto category)
        {
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
