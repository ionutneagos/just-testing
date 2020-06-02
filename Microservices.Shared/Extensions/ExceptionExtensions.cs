using Microservices.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Microservices.Shared
{
    public static class ExceptionExtensions
    {
        public static ActionResult GetResponse(this Exception exception, ILogger log)
        {
            var appException = exception is AppException ? (AppException)exception : new AppException(500, "Internal server error", exception);

            log.LogError(appException.ToString());

            return new ContentResult
            {
                Content = appException.Message,
                StatusCode = appException.Code,
                ContentType = "plain/text",
            };
        }
    }
}
