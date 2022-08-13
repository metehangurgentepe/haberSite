namespace haber1;

using haber1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<string> _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles ?? new string[] { };
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;
        var roles = _roles.ToList();
        // authorization

        //Admin ise her türlü kabul et
        var user = (User)context.HttpContext.Items["User"];
        if (user != null && user.UserType == 0) //Admin ise
            return;

        List<int> permissions = new List<int>();


        var endPointMetadata = context.ActionDescriptor.EndpointMetadata;

        //Yetki kontrolü
        bool yetkisiVar = false;
        if(user!=null)
        yetkisiVar = roles.FirstOrDefault(o=> o == user.UserType.ToString()) != null;

        if (user == null || roles.Any() && !yetkisiVar)
        {
            // not logged in or role not authorized
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}