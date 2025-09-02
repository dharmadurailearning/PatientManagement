using System.Net;
using System.Text.Json;

namespace PatientManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try { await _next(context); }
            catch (KeyNotFoundException ex)
            {
                await WriteProblem(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await WriteProblem(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteProblem(context, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static Task WriteProblem(HttpContext ctx, HttpStatusCode code, string message)
        {
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)code;
            var payload = JsonSerializer.Serialize(new
            {
                status = (int)code,
                title = Enum.GetName(typeof(HttpStatusCode), code),
                detail = message
            });
            return ctx.Response.WriteAsync(payload);
        }
    }
}