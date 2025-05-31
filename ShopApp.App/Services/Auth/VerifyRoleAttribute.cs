using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApp.Data.Enums;

namespace ShopApp.Services.Auth;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class VerifyRoleAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _allowedRoles;

    public VerifyRoleAttribute(params EmployeeRoles[] allowedRoles)
    {
        _allowedRoles = allowedRoles.Select(r => r.ToString()).ToArray();
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        if (!IsUserAuthenticated(user))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!TryGetUsername(user, out var username))
        {
            context.Result = new ForbidResult();
            return;
        }

        
        if (!TryGetEmployeeService(httpContext, out var service))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (service != null && !await UserHasPermissionByRole(service, username))
        {
            context.Result = new JsonResult(new { error = "Method is forbidden for this role" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
    
    private bool IsUserAuthenticated(IPrincipal user)
    {
        return user.Identity != null && user.Identity.IsAuthenticated;
    }

    private bool TryGetUsername(IPrincipal user, out string? username)
    {
        username = user.Identity?.Name;
        return !string.IsNullOrWhiteSpace(username);
    }

    private bool TryGetEmployeeService(HttpContext context, out IEmployeeService? employeeService)
    {
        employeeService = context.RequestServices.GetService<IEmployeeService>();
        return employeeService != null;
    }

    private async Task<bool> UserHasPermissionByRole(IEmployeeService employeeService, string username)
    {
        var employee = await employeeService.GetByUsername(username);
        if (employee == null) return false;

        var role = await employeeService.GetEmployeeRole(employee.IdEmployee);
        return _allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }
}