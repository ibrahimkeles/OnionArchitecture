using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.TodoLists.DeleteTodoList
{
    public class DeleteTodoListRequest : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteTodoListHandler : IRequestHandler<DeleteTodoListRequest, Result>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IDistributedCache _cache;
        public DeleteTodoListHandler(ITodoListRepository todoListRepository, IDistributedCache cache)
        {
            _todoListRepository = todoListRepository;
            _cache = cache;
        }

        public async Task<Result> Handle(DeleteTodoListRequest request, CancellationToken cancellationToken)
        {
            TodoList? todoList = await _todoListRepository.GetAsync(x => x.Id == request.Id);
            if (todoList is null) return new Result(false, "Beklenmedik bir hata ile karşılaşıldı silenecek bir liste bulunamadı!");
            _todoListRepository.PassiveDelete(todoList);
            await _todoListRepository.SaveAsync();
            await _cache.RemoveAsync($"todo_list{todoList.User}");
            return new Result(true, "Silme işlemi başarılı");
        }
    }
}
