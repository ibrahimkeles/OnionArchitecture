using AutoMapper;
using OnionArchitecture.Application.Features.Commands.TodoItems.CreateTodoItem;
using OnionArchitecture.Application.Features.Commands.TodoItems.UpdateTodoItem;
using OnionArchitecture.Domain.Entites;

namespace OnionArchitecture.Application.Mappers
{
    public class TodoItemMapper : Profile
    {
        public TodoItemMapper()
        {
            CreateMap<TodoItem, CreateTodoItemRequest>().ReverseMap();
            CreateMap<TodoItem, UpdateTodoItemRequest>().ReverseMap();
        }
    }
}
