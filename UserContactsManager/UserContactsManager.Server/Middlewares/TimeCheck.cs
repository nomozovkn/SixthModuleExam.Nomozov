using System.Diagnostics;

namespace UserContactsManager.Server.Middlewares;

public class TimeCheck
{
    private readonly RequestDelegate _next;

    public TimeCheck(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var currentHour = DateTime.Now.Hour;

        if (!(currentHour >= 9 && currentHour<=18 || context.Request.Path.ToString().Contains("get")))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "This API works between 9am and 6pm. "
            });

            return;
        }

        await _next(context);
    }
}
