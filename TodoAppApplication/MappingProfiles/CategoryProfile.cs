using AutoMapper;
using TodoAppApplication.DTOs.Category;
using TodoAppCore.Models;

namespace TodoAppApplication.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<Category, CategoryResponseDto>();
        }
    }
}
