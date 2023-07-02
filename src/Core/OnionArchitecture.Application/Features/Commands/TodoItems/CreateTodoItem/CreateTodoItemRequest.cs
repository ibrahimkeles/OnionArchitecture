using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.TodoItems.CreateTodoItem
{
    public class CreateTodoItemRequest : IRequest<Result>
    {
        public int TodoListId { get; set; }
        public string Title { get; set; }
    }
    public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemRequest, Result>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public CreateTodoItemHandler(ITodoItemRepository todoItemRepository, IMapper mapper, IDistributedCache cache)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = new();
            _mapper.Map(request, todoItem);
            await _todoItemRepository.AddAsync(todoItem);
            await _todoItemRepository.SaveAsync();
            await _cache.RemoveAsync($"todo_item{request.TodoListId}");
            return new Result(true, "Ekleme işlemi başarılı");
        }
    }
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemRequest>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.TodoListId).NotEqual(0).WithMessage("Beklenmedik bir hata oluştu! Eklenecek bir todo listesi bulunamadı!");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Todo boş geçilemez!");
        }
    }
}
