using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApplication.Infra
{
    public static class HttpRequestExtensions
    {
        public static IEtagHandlerFeature GetEtagHandler(this HttpRequest request)
        {
            return request.HttpContext.Features.Get<IEtagHandlerFeature>();
        }
    }
}
