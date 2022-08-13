namespace haber1;

using haber1.Context;
using Microsoft.Extensions.Options;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    HaberContext haberContext;
    JwtUtils jwtUtils;
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
        jwtUtils = new JwtUtils();
        haberContext = new HaberContext();
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userName = jwtUtils.ValidateJwtToken(token);
        if (userName != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = haberContext.Users.SingleOrDefault(o=>o.Username== userName);
        }


        await _next(context);
    }
}