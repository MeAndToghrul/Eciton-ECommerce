using Eciton.Application.ResponceObject.Enums;
using Eciton.Application.ResponceObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
namespace Eciton.API.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _requiredRoleName;

    public AuthAttribute(string requiredRoleName)
    {
        _requiredRoleName = requiredRoleName;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new JsonResult(new Response(ResponseStatusCode.Unauthorized, "User is not authenticated."))
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(roleClaim))
        {
            context.Result = new JsonResult(new Response(ResponseStatusCode.Forbidden, "Role information is missing in the token."))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        if (!string.Equals(roleClaim, _requiredRoleName, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new JsonResult(new Response(ResponseStatusCode.Forbidden, "You do not have permission to perform this action."))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}
