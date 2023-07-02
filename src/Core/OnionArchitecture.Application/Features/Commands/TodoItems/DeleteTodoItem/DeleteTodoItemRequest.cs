using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.TodoItems.DeleteTodoItem
{
    public class DeleteTodoItemRequest : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteTodoItemHandler : IRequestHandler<DeleteTodoItemRequest, Result>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IDistributedCache _cache;

        public DeleteTodoItemHandler(ITodoItemRepository todoItemRepository, IDistributedCache cache)
        {
            _todoItemRepository = todoItemRepository;
            _cache = cache;
        }

        public async Task<Result> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
        {
            TodoItem? todoItem = await _todoItemRepository.GetAsync(x => x.Id == request.Id);
            if (todoItem is null) return new Result(false, "Beklenmedik bir hata oluştu, todo bulunamadı!");

            _todoItemRepository.PassiveDelete(todoItem);
            await _todoItemRepository.SaveAsync();
            await _cache.RemoveAsync($"todo_item{todoItem.TodoListId}");
            return new Result(true, "Silme işlemi başarılı!");

        }
    }
}
