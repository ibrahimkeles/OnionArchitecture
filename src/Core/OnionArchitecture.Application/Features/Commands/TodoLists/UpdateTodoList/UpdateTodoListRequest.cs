using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using System.Text.Json.Serialization;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.TodoLists.UpdateTodoList
{
    public class UpdateTodoListRequest : IRequest<Result>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        public string Title { get; set; }
    }
    public class UpdateTodoListHandler : IRequestHandler<UpdateTodoListRequest, Result>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public UpdateTodoListHandler(ITodoListRepository todoListRepository, IMapper mapper, IDistributedCache cache)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result> Handle(UpdateTodoListRequest request, CancellationToken cancellationToken)
        {
            TodoList? todoList = await _todoListRepository.GetAsync(x => x.Id != request.Id && x.UserId == request.UserId && x.Title == request.Title);
            if (todoList is not null) return new Result(false, $"{request.Title} başlıklı todo list zaten daha önceden eklenmiştir!");

            todoList = await _todoListRepository.GetAsync(x => x.Id == request.Id);
            if (todoList is null) return new Result(false, "Beklenmedik hata güncellenecek bir liste bulunamadı!");

            _mapper.Map(request, todoList);
            _todoListRepository.Update(todoList);
            await _todoListRepository.SaveAsync();
            await _cache.RemoveAsync($"todo_list{request.UserId}");
            return new Result(true, "Liste başarılı bir şekilde güncellendi");
        }
    }
    public class UpdateTodoListValidator : AbstractValidator<UpdateTodoListRequest>
    {
        public UpdateTodoListValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Liste adı boş geçilemez!");
        }
    }
}
