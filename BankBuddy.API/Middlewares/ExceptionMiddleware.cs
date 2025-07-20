
using BankBuddy.Application.Commons;
using System.Net;
using System.Text.Json;

namespace BankBuddy.API.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
			try
			{
                await _next(httpContext);
			}
			catch (AppException ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, ex.StatusCode);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex.ToString(), (int)HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}
