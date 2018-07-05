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