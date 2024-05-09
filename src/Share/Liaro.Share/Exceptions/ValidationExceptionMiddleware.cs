using Liaro.Share.Abstract.Exceptions;

namespace Liaro.Share.Exceptions;

public sealed class ValidationsExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationsException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.Headers.Add("content-type", "application/json");
            var errorType = ToUnderscoreCase(ex.GetType().Name.Replace("Exception", string.Empty));
            var json = JsonSerializer.Serialize(new { ErrorType = errorType, ex.Message, ex.Errors });
            await context.Response.WriteAsync(json);
        }
    }
    public static string ToUnderscoreCase(string value)
        => string.Concat((value ?? string.Empty)
            .Select((x, i) =>
                i > 0 &&
                char.IsUpper(x) &&
                !char.IsUpper(value[i - 1]) ? $"_{x}" : x.ToString()))
                .ToLower();
}