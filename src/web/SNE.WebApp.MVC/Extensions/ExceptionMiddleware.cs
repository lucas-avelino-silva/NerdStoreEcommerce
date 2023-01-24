using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace SNE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        //fiz esse middleware e preciso registrar ele no program
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate net)
        {
            _next = net;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsinc(httpContext, ex.StatusCode);
            }
            catch(ValidationApiException ex)
            {
                HandleRequestExceptionAsinc(httpContext, ex.StatusCode);
            }
            catch(ApiException ex)
            {
                HandleRequestExceptionAsinc(httpContext, ex.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleRCircuitBreakerException(httpContext);
            }
        }

        public static void HandleRequestExceptionAsinc(HttpContext context, HttpStatusCode statusCode)
        {
            if(statusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/Login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        public static void HandleRCircuitBreakerException(HttpContext context)
        {
            context.Response.Redirect("/sistema-indisponivel");
        }
    }
}
