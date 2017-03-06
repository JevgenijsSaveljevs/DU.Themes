using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Filters;

namespace DU.Themes
{
    public class ErrorHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ValidationException)
            {
                var ex = actionExecutedContext.Exception as ValidationException;

                actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                actionExecutedContext.Response.Content = new StringContent(Json.Encode(new
                {
                    GeneralError = ex.Message,
                    ValidationErrors = ex.Errors
                }));

                return;
            }
        }
    }
}