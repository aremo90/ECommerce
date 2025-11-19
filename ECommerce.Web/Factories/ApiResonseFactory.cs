using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Factories
{
    public static class ApiResonseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var errors = actionContext.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(X => X.Key, X => X.Value?.Errors
                        .Select(x => x.ErrorMessage).ToArray());

            var errorResponse = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Extensions = 
                {
                    { "errors", errors }
                }

            };
            return new BadRequestObjectResult(errorResponse);
        }
    }
}
