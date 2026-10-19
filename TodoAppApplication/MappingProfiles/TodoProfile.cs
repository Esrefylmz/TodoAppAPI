using AutoMapper;
using TodoAppDTOs.Todo;
using TodoAppCore.Models;

namespace TodoAppApplication.MappingProfiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<CreateTodoDto, Todo>();

            CreateMap<UpdateTodoDto, Todo>();

            CreateMap<Todo, TodoResponseDto>();
        }

    }
}
