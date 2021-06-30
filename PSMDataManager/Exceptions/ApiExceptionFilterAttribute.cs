﻿using System.Net.Http;
using System.Web.Http.Filters;

namespace PSMDataManager.Exceptions
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ApiException exception)
            {
                context.Response = context.Request.CreateErrorResponse(exception.StatusCode, exception.Message);
            }
        }
    }
}