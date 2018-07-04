using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListApplication.Models;

namespace TodoListApplication.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/lists")]
    public class TodoListController : Controller
    {
        public IActionResult Index()
        {
            return Ok(Enumerable.Range(0, 2).Select(_ => new TodoList()
            {
                Id = "1"
            }));
        }
    }
}