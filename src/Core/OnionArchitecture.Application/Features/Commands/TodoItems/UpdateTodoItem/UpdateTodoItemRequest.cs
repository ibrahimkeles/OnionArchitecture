using AutoMapper;
using FluentValidation;
using MediatR;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.TodoItems.UpdateTodoItem
{
    public class UpdateTodoItemRequest : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemRequest, Result>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        public UpdateTodoItemHandler(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
        {
            TodoItem? todoItem = await _todoItemRepository.GetAsync(x => x.Id == request.Id);
            if (todoItem is null) return new Result(false, "Beklenmedik bir hata oluştu, todo bulunamadı!");
            _mapper.Map(request, todoItem);
            _todoItemRepository.Update(todoItem);
            await _todoItemRepository.SaveAsync();
            return new Result(true, "Güncelleme işlemi başarılı!");
        }
    }
    public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemRequest>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(x => x.Id).NotEqual(0).WithMessage("Beklenmedik bir hata oluştu! Todo buluanamadı");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Todo boş geçilemez!");
        }
    }
}
