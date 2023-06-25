using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Repositories;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Queries.TodoItems.GetAllTodoItems
{
    public class GetAllTodoItemsRequest : IRequest<Result>
    {
        public int TodoListId { get; set; }
    }
    public class GetAllTodoItemsHandler : IRequestHandler<GetAllTodoItemsRequest, Result>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetAllTodoItemsHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<Result> Handle(GetAllTodoItemsRequest request, CancellationToken cancellationToken)
        {
            var data = await _todoItemRepository.GetAll(
                  x => x.TodoListId == request.TodoListId
                  && x.IsDeleted == false)
                  .Select(x => new
                  {
                      Id = x.Id,
                      Title = x.Title,
                      IsCompleted = x.IsCompleted
                  }).ToListAsync();
            return new Result(true, "Listeleme başarılı", data);
        }
    }
}
