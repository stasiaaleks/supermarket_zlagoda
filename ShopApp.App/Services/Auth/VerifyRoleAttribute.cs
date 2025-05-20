using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApp.Data.Enums;

namespace ShopApp.Services.Auth;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class VerifyRoleAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _allowedRole;

    public VerifyRoleAttribute(EmployeeRoles allowedRole)
    {
        _allowedRole = allowedRole.ToString().ToLower();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // TODO: needs tidying up
        var httpContext = context.HttpContext;
        var user = httpContext.User;
        var employeeService = httpContext.RequestServices.GetService<IEmployeeService>();
        
        if (user.Identity == null || employeeService == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var username = user.Identity.Name;
        var employee = await employeeService.GetEmployeeByUsernameAsync(username);
        if (employee == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var role = await employeeService.GetEmployeeRoleAsync(employee.IdEmployee);
        if (!string.Equals(_allowedRole, role, StringComparison.OrdinalIgnoreCase)) // case-insensitive role
        {
            context.Result = new ForbidResult();

        }
    }
}