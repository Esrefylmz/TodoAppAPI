using AutoMapper;
using TodoAppAPI.DTOs.Todo;
using TodoAppCore.Models;

namespace TodoAppAPI.MappingProfiles
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
