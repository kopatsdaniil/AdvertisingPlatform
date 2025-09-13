namespace AdvertisingPlatforms.Middleware;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private const string ApiKeyName = "Authorization";

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        var apiKey = extractedApiKey.ToString();

        var configuredApiKey = configuration.GetValue<string>("ApiKey");
        if (!apiKey.Equals(configuredApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }

        await next(context);
    }
}