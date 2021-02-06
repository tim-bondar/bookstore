using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IActionResultExecutor<ObjectResult> _executor;
        private readonly ILogger _logger;
        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        public ExceptionHandlerMiddleware(RequestDelegate next, IActionResultExecutor<ObjectResult> executor,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _executor = executor;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorResult = HandleException(ex);

                if (errorResult.code == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError(ex,
                        $"An unhandled exception has occurred while executing the request. " +
                        $"Url: {context.Request.GetDisplayUrl()}. Request Data: {GetRequestData(context)}");
                }

                var routeData = context.GetRouteData();
                ClearCacheHeaders(context.Response);
                var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);
                var result = new ObjectResult(errorResult.response)
                {
                    StatusCode = (int)errorResult.code,
                };

                await _executor.ExecuteAsync(actionContext, result);
            }
        }

        private static (ErrorResponse response, HttpStatusCode code) HandleException(Exception ex)
        {
            // Custom exception handling
            return ex switch
            {
                BookNotFoundException bookEx => (new ErrorResponse(bookEx.Message), HttpStatusCode.NotFound),
                _ => (new ErrorResponse("Error processing request. Server error."), HttpStatusCode.InternalServerError)
            };
        }

        private static string GetRequestData(HttpContext context)
        {
            var sb = new StringBuilder();

            if (context.Request.HasFormContentType && context.Request.Form.Any())
            {
                sb.Append("Form variables:");
                foreach (var x in context.Request.Form)
                {
                    sb.AppendFormat("Key={0}, Value={1}<br/>", x.Key, x.Value);
                }
            }

            sb.AppendLine("Method: " + context.Request.Method);

            return sb.ToString();
        }

        private static void ClearCacheHeaders(HttpResponse response)
        {
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
        }

        [DataContract(Name = "ErrorResponse")]
        public class ErrorResponse
        {
            public ErrorResponse(string message)
            {
                Message = message;
            }

            [DataMember(Name = "Message")]
            public string Message { get; }
        }
    }
}
