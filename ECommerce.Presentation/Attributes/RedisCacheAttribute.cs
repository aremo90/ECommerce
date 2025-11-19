using ECommerce.ServiceAbstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationinMin;

        public RedisCacheAttribute(int DurationinMin = 5)
        {
            _durationinMin = DurationinMin;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // get CacheService
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // check if cache exists
            var cacheKey = CreateCacheKey(context.HttpContext.Request);




            // if exists => return cached value
            var cachedValue = await cacheService.GetAsync(cacheKey);
            if (cachedValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cachedValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // if not exists =>  Proceed to the action
            var ExecutedContext = await next.Invoke();

            if (ExecutedContext.Result is OkObjectResult okObjectResult)
            {
                // Store the result in cache
                await cacheService.SetAsync(cacheKey, okObjectResult.Value!, TimeSpan.FromMinutes(5));
            }

        }


        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);

            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }
            return Key.ToString();
        }

    }

}
