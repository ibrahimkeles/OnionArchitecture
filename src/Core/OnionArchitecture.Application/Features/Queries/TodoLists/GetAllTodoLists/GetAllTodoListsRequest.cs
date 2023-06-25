using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Repositories;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Queries.TodoLists.GetAllTodoLists
{
    public class GetAllTodoListsRequest : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetAllTodoListsHandler : IRequestHandler<GetAllTodoListsRequest, Result>
    {
        private readonly ITodoListRepository _todoListRepository;

        public GetAllTodoListsHandler(ITodoListRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<Result> Handle(GetAllTodoListsRequest request, CancellationToken cancellationToken)
        {
            var data = await _todoListRepository
                    .GetAll(
                    x => x.IsDeleted == false
                    && x.UserId == request.UserId)
                   .Select(x => new
                   {
                       Id = x.Id,
                       Title = x.Title
                   }).ToListAsync();
            return new Result(true, "Listeleme başarılı", data);
        }
    }
}
