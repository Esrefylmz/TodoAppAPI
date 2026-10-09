using AutoMapper;
using TodoAppAPI.DTOs.Category;
using TodoAppCore.Models;

namespace TodoAppAPI.MappingProfiles
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
