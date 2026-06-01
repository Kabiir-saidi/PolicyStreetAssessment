using PolicyStreetAssessment.Data;
using PolicyStreetAssessment.Models;

namespace PolicyStreetAssessment.Middleware;

public class AuditLogMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        // Buffer the request body so we can read it
        context.Request.EnableBuffering();

        string parameters = context.Request.QueryString.Value ?? string.Empty;

        if (context.Request.ContentLength > 0 &&
            context.Request.ContentType?.Contains("application/json") == true)
        {
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(body))
                parameters = string.IsNullOrWhiteSpace(parameters)
                    ? body
                    : parameters + " | " + body;
        }

        await _next(context);

        var log = new AuditLog
        {
            HttpMethod = context.Request.Method,
            Endpoint = context.Request.Path.Value ?? string.Empty,
            Parameters = parameters.Length > 4000 ? parameters[..4000] : parameters,
            StatusCode = context.Response.StatusCode,
            CalledAt = DateTime.UtcNow,
            IpAddress = context.Connection.RemoteIpAddress?.ToString()
        };

        db.AuditLogs.Add(log);
        await db.SaveChangesAsync();
    }
}
