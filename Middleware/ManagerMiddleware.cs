using System.Net;
using Newtonsoft.Json;

namespace NetKubernetes.Middleware;

public class ManagerMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ManagerMiddleware> _logger;

  public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await ManagerExceptionAsync(context, ex, _logger);
    }
  }

  private async Task ManagerExceptionAsync(HttpContext context, Exception exception, ILogger<ManagerMiddleware> logger)
  {
    object? errors = null;

    switch (exception)
    {
      case MiddlewareException me:
        logger.LogError(exception, "Middleware Error");
        errors = me.Errors;
        context.Response.StatusCode = (int)me.StatusCode;
        break;
      
      case Exception e:
        logger.LogError(exception, "Server Error");
        errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        break;
    }

    context.Response.ContentType = "application/json";
    var result = string.Empty;

    if (errors != null)
    {
      result = JsonConvert.SerializeObject(new { errors });
    }

    await context.Response.WriteAsync(result);
  }
}