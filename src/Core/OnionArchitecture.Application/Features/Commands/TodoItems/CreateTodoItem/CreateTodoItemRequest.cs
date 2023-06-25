using AutoMapper;
using FluentValidation;
using MediatR;
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

        public CreateTodoItemHandler(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = new();
            _mapper.Map(request, todoItem);
            await _todoItemRepository.AddAsync(todoItem);
            await _todoItemRepository.SaveAsync();
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
