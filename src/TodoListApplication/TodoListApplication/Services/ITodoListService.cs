using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TodoListApplication.Models;

namespace TodoListApplication.Services
{
    public interface ITodoListService
    {
        Task<TodoList> GetTodoListAsync(string id, CancellationToken ct);
        Task<IEnumerable<TodoList>> GetTodoListsAsync(QueryOptions options, CancellationToken ct);
        Task<(bool success, HttpStatusCode? error)> CreateTodoListAsync(TodoList input);
        Task<(bool success, HttpStatusCode? error)> CreateTodoTaskAsync(string todoListId, TodoTask input);
    }
}
