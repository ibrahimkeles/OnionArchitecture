using AutoMapper;
using FluentValidation;
using MediatR;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using System.Text.Json.Serialization;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.TodoLists.CreateTodoList
{
    public class CreateTodoListRequest : IRequest<Result>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string Title { get; set; }
    }
    public class CreateTodoListHandler : IRequestHandler<CreateTodoListRequest, Result>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;
        public CreateTodoListHandler(ITodoListRepository todoListRepository, IMapper mapper)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateTodoListRequest request, CancellationToken cancellationToken)
        {
            TodoList? todoList = await _todoListRepository.GetAsync(x => x.UserId == request.UserId && x.Title == request.Title);
            if (todoList is not null) return new Result(false, $"{request.Title} başlıklı todo list zaten daha önceden eklenmiştir!");

            todoList = new();
            _mapper.Map(request, todoList);
            await _todoListRepository.AddAsync(todoList);
            await _todoListRepository.SaveAsync();
            return new Result(true, $"{todoList.Title} adlı liste başarılı bir şekilde oluşturulmuştur!");
        }
    }
    public class CreateTodoListValidator : AbstractValidator<CreateTodoListRequest>
    {
        public CreateTodoListValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Liste adı boş geçilemez!");
        }
    }
}
