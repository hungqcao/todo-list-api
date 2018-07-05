using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TodoListApplication.Models;

namespace TodoListApplication.ApiFilters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public ExceptionFilter(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            // Handle unexpected exception
            var apiError = new ApiErrorResponse();
            if (this.hostingEnvironment.IsDevelopment())
            {
                apiError.Message = context.Exception.Message;
            }
            else
            {
                apiError.Message = "Error occured!";
            }

            context.Result = new ObjectResult(apiError)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
