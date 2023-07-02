using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OnionArchitecture.Application.Repositories;
using System.Text;
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
        private readonly IDistributedCache _cache;
        public GetAllTodoListsHandler(ITodoListRepository todoListRepository, IDistributedCache cache)
        {
            _todoListRepository = todoListRepository;
            _cache = cache;
        }

        public async Task<Result> Handle(GetAllTodoListsRequest request, CancellationToken cancellationToken)
        {
            var cacheTodoItems = await _cache.GetAsync($"todo_list{request.UserId}");
            if (cacheTodoItems != null)
            {
                string serializeItems = Encoding.UTF8.GetString(cacheTodoItems);
                var cacheData = JsonConvert.DeserializeObject<List<GetAllTodoListsResponse>>(serializeItems);
                return new Result(true, "Listeleme başarılı", cacheData);
            }
            var data = await _todoListRepository
                    .GetAll(
                    x => x.IsDeleted == false
                    && x.UserId == request.UserId)
                   .Select(x => new GetAllTodoListsResponse
                   {
                       Id = x.Id,
                       Title = x.Title
                   }).ToListAsync();
            await _cache.SetAsync($"todo_list{request.UserId}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            return new Result(true, "Listeleme başarılı", data);
        }
    }
    public class GetAllTodoListsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
