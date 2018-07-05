using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApplication.Infra;

namespace TodoListApplication.ApiFilters
{
    public class EtagFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Features.Set<IEtagHandlerFeature>(
                new EtagHandlerFeature(context.HttpContext.Request.Headers));

            var executed = await next();

            var result = executed?.Result as ObjectResult;

            var etag = (result?.Value as IEtaggable)?.GetEtag();
            var etagForCollection = EtagHelpers.GetEtagForCollection((result?.Value as IEnumerable<IEtaggable>));

            if (string.IsNullOrEmpty(etag) && string.IsNullOrEmpty(etagForCollection)) return;

            if (!string.IsNullOrEmpty(etag) && !etag.Contains('"'))
            {
                etag = $"\"{etag}\"";
                context.HttpContext.Response.Headers.Add("ETag", etag);
            }
            else if (!string.IsNullOrEmpty(etagForCollection) && !etagForCollection.Contains('"'))
            {
                etag = $"\"{etagForCollection}\"";
                context.HttpContext.Response.Headers.Add("ETag", etagForCollection);
            }

            // If a response body was set so that we would add
            // the ETag header, but the status code is 304,
            // the body should be removed.
            if (result.StatusCode == 304)
            {
                result.Value = null;
            }

            return;
        }
    }
}
