using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                Console.WriteLine("request _next");
                await _next(httpContext);
                Console.WriteLine("respose _next");
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException httpRequestException)
        {

            switch (httpRequestException.StatusCode)
            {
                case HttpStatusCode.Unauthorized: 
                    context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                    break;
                default:
                     context.Response.StatusCode = (int)httpRequestException.StatusCode;
                    break;
            }
        }
    }
}
