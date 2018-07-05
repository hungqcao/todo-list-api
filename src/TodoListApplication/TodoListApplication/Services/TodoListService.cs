using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TodoListApplication.Models;

namespace TodoListApplication.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly ConcurrentDictionary<string, TodoList> fakeDatabase;

        public TodoListService()
        {
            this.fakeDatabase = new ConcurrentDictionary<string, TodoList>();
            // Setup some fake data
            this.fakeDatabase.TryAdd("List1", new TodoList
            {
                Id = "List1",
                Description = "TodoList 1",
                Name = "TodoList 1",
                Tasks = new List<TodoTask>()
                {
                    new TodoTask
                    {
                        Id = "Task1",
                        Name = "Task 1",
                        Completed = false
                    }
                }
            });
        }

        public async Task<(bool success, HttpStatusCode? error)> CreateTodoListAsync(TodoList input)
        {
            if (input == null) throw new ArgumentNullException("Input is null");

            if (this.fakeDatabase.ContainsKey(input.Id)) return (false, HttpStatusCode.Conflict);

            return (this.fakeDatabase.TryAdd(input.Id, input), null);
        }

        public async Task<(bool success, HttpStatusCode? error)> CreateTodoTaskAsync(string todoListId, TodoTask input)
        {
            if (string.IsNullOrEmpty(todoListId)) throw new ArgumentNullException("TodoListId is null");
            if (input == null) throw new ArgumentNullException("Input is null");

            if (this.fakeDatabase.TryGetValue(todoListId, out TodoList todoList))
            {
                if (todoList.Tasks.FirstOrDefault(t => t.Id.Equals(input.Id)) != null)
                {
                    todoList.Tasks.Add(input);
                    
                    return (true, null);
                }
                else
                {
                    return (false, HttpStatusCode.Conflict);
                }
            }
            else
            {
                return (false, HttpStatusCode.NotFound);
            }
        }

        public async Task<TodoList> GetTodoListAsync(string id, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id is null");

            if (this.fakeDatabase.TryGetValue(id, out TodoList todoList))
            {
                return todoList;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<TodoList>> GetTodoListsAsync(QueryOptions options, CancellationToken ct)
        {
            var allTodos = this.fakeDatabase.Select(t => t.Value);

            if (options == null)
            {
                return allTodos;
            }
            else
            {
                IEnumerable<TodoList> queriedTodos = null;
                if (!string.IsNullOrEmpty(options.SearchString))
                {
                    queriedTodos = allTodos.Where(t => t.Name.Contains(options.SearchString));
                }
                else
                {
                    queriedTodos = allTodos;
                }

                if (options.Skip.HasValue)
                {
                    queriedTodos = queriedTodos.Skip(options.Skip.Value);
                } // else do nothing

                if (options.Limit.HasValue)
                {
                    queriedTodos = queriedTodos.Take(options.Limit.Value);
                }// else do nothing

                return queriedTodos;
            }
        }
    }
}
