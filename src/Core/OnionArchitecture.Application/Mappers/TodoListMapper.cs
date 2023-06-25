using AutoMapper;
using OnionArchitecture.Application.Features.Commands.TodoLists.CreateTodoList;
using OnionArchitecture.Application.Features.Commands.TodoLists.UpdateTodoList;
using OnionArchitecture.Domain.Entites;

namespace OnionArchitecture.Application.Mappers
{
    public class TodoListMapper : Profile
    {
        public TodoListMapper()
        {
            CreateMap<TodoList, CreateTodoListRequest>().ReverseMap();
            CreateMap<TodoList, UpdateTodoListRequest>().ReverseMap();
        }
    }
}
