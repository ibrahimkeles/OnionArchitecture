using MediatR;
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

        public DeleteTodoItemHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<Result> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
        {
            TodoItem? todoItem = await _todoItemRepository.GetAsync(x => x.Id == request.Id);
            if (todoItem is null) return new Result(false, "Beklenmedik bir hata oluştu, todo bulunamadı!");

            _todoItemRepository.PassiveDelete(todoItem);
            await _todoItemRepository.SaveAsync();
            return new Result(true, "Silme işlemi başarılı!");

        }
    }
}
