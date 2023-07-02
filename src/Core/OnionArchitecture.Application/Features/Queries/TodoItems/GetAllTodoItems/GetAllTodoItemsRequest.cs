using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OnionArchitecture.Application.Repositories;
using System.Text;
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
        private readonly IDistributedCache _cache;
        public GetAllTodoItemsHandler(ITodoItemRepository todoItemRepository, IDistributedCache cache)
        {
            _todoItemRepository = todoItemRepository;
            _cache = cache;
        }

        public async Task<Result> Handle(GetAllTodoItemsRequest request, CancellationToken cancellationToken)
        {
            var cacheTodoItems = await _cache.GetAsync($"todo_item{request.TodoListId}");
            if (cacheTodoItems != null)
            {
                string serializeItems = Encoding.UTF8.GetString(cacheTodoItems);
                var cacheData = JsonConvert.DeserializeObject<List<GetAllTodoItemsResponse>>(serializeItems);
                return new Result(true, "Listeleme başarılı", cacheData);
            }
            var data = await _todoItemRepository.GetAll(
                  x => x.TodoListId == request.TodoListId
                  && x.IsDeleted == false)
                  .Select(x => new GetAllTodoItemsResponse
                  {
                      Id = x.Id,
                      Title = x.Title,
                      IsCompleted = x.IsCompleted
                  }).ToListAsync();
            await _cache.SetAsync($"todo_item{request.TodoListId}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            return new Result(true, "Listeleme başarılı", data);
        }
    }
    public class GetAllTodoItemsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
