using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListApplication.Infra;
using TodoListApplication.Models;
using TodoListApplication.Services;

namespace TodoListApplication.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("/lists")]
    public class TodoListController : Controller
    {
        private readonly ITodoListService todoListService;
        public TodoListController(ITodoListService todoListService)
        {
            this.todoListService = todoListService;
        }

        /// <summary>
        /// Search todo list API
        /// </summary>
        /// <param name="options"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetListsAsync))]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "searchString", "limit", "skip" })]
        [Etag]
        public async Task<IActionResult> GetListsAsync([FromQuery] QueryOptions options, CancellationToken ct)
        {
            var result = await this.todoListService.GetTodoListsAsync(options, ct);
            if (!Request.GetEtagHandler().NoneMatch(result))
            {
                return StatusCode(304, result);
            }
            else
            {
                return Ok(result);
            }
        }

        /// <summary>
        /// Get Todo list by Id
        /// </summary>
        /// <param name="id">TodoList Id</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetListByIdAsync))]
        [Etag]
        public async Task<IActionResult> GetListByIdAsync(string id, CancellationToken ct)
        {
            var todoList = await this.todoListService.GetTodoListAsync(id, ct);
            if(todoList == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(todoList);
            }
        }

        /// <summary>
        /// Create new Todo list
        /// </summary>
        /// <param name="todoList"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(CreateTodoListAsync))]
        public async Task<IActionResult> CreateTodoListAsync([FromBody] TodoList todoList)
        {
            var result = await this.todoListService.CreateTodoListAsync(todoList);
            if (result.success)
            {
                return Created(Url.Link(nameof(GetListByIdAsync), new { id = todoList.Id }), todoList);
            }
            else
            {
                if(result.error == System.Net.HttpStatusCode.Conflict)
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        /// <summary>
        /// Get all tasks of a Todo list
        /// </summary>
        /// <param name="todoListId">Todo List Id</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{todoListId}/tasks", Name = nameof(GetTodoTasksAsync))]
        public async Task<IActionResult> GetTodoTasksAsync(string todoListId, CancellationToken ct)
        {
            var result = await this.todoListService.GetTodoListAsync(todoListId, ct);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result.Tasks);
            }
        }

        /// <summary>
        /// Create a new task under a Todo list
        /// </summary>
        /// <param name="todoListId">Todo list Id</param>
        /// <param name="todoTask"></param>
        /// <returns></returns>
        [HttpPost("{todoListId}/tasks", Name = nameof(CreateTodoTaskAsync))]
        public async Task<IActionResult> CreateTodoTaskAsync(string todoListId, [FromBody] TodoTask todoTask)
        {
            var result = await this.todoListService.CreateTodoTaskAsync(todoListId, todoTask);
            if (result.success)
            {
                return Created(Url.Link(nameof(GetListByIdAsync), todoTask.Id), todoTask);
            }
            else
            {
                if (result.error == System.Net.HttpStatusCode.Conflict)
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}