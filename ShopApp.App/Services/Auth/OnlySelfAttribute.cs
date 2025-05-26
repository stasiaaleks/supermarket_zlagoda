using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApp.Data.Enums;

namespace ShopApp.Services.Auth;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class OnlySelfAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _paramName;
    private readonly string[] _roles;

    public OnlySelfAttribute(string paramName, params EmployeeRoles[] roles)
    {
        _paramName = paramName;
        _roles = roles.Select(r => r.ToString()).ToArray();
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        if (!TryGetEmployeeId(user, out var emplId))
        {
            context.Result = new ForbidResult();
            return Task.CompletedTask;
        }

        var routeData = context.RouteData.Values;
        var targetId = routeData[_paramName]?.ToString();

        // if user's role is NOT in the restricted list, allow
        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        if (role == null || !_roles.Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        if (emplId != targetId)
        {
            context.Result = new ObjectResult(new  { Error = $"Employees with role {role} are not allowed to access other employees` data" })
            { StatusCode = StatusCodes.Status403Forbidden };
        }

        return Task.CompletedTask;
    }

    private bool TryGetEmployeeId(IPrincipal user, out string? emplId)
    {
        emplId = null;

        if (user is ClaimsPrincipal claimsPrincipal)
        {
            emplId = claimsPrincipal.FindFirst("EmployeeId")?.Value;
            if (emplId != null)
            {
                return true;
            }
        }

        return false;
    }
}