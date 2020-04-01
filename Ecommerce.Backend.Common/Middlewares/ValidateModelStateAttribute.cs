using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.Backend.API.Middlewares
{
  public class ValidateModelStateAttribute : IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (!context.ModelState.IsValid)
      {
        context.Result = new BadRequestObjectResult(context.ModelState.CreateErrorResponse());
      }
      else
      {
        await next();
      }
    }
  }
}