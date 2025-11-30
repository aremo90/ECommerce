using ECommerce.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next , ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await HandleNotFoundException(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong");
                Console.WriteLine(ex.Message);
                var Problem = new ProblemDetails() 
                {
                    Title = "An unexpected error occurred!",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch 
                    { 
                        NotFoundExceptions => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }

                };
                context.Response.StatusCode = Problem.Status.Value;
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }

        private static async Task HandleNotFoundException(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "Resource Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The Resouce you are looking for is not found",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
