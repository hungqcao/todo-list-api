using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApplication.Controllers.v1;
using TodoListApplication.Models;
using TodoListApplication.Services;
using Xunit;

namespace TodoListTest
{
    public class TestTodoListControllers
    {
        private readonly Mock<ITodoListService> mockTodoListService;
        private TodoListController target;

        public TestTodoListControllers()
        {
            this.mockTodoListService = new Mock<ITodoListService>();
            this.mockTodoListService.Setup(_ => _.GetTodoListAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(
                new TodoList
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
            this.target = new TodoListController(this.mockTodoListService.Object);
        }

        [Fact]
        public async Task Test_GetTodoTasksAsync()
        {
            var result = await this.target.GetListByIdAsync("List1", new System.Threading.CancellationToken());

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Test_GetTodoTasksAsync_NotFound()
        {
            this.mockTodoListService.Setup(_ => _.GetTodoListAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(default(TodoList));
            var result = await this.target.GetListByIdAsync("List1", new System.Threading.CancellationToken());

            var okResult = result as NotFoundResult;
            Assert.NotNull(okResult);
            Assert.Equal(404, okResult.StatusCode);
        }
    }
}
