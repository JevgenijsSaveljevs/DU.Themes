using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Filters;
using FluentValidation;

namespace DU.Themes.Api
{
    public class ValidationApiFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ValidationException)
            {
                var ex = actionExecutedContext.Exception as ValidationException;

                actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                actionExecutedContext.Response.Content = new StringContent(Json.Encode(new
                {
                    GeneralError = "Request didn't passed validation",
                    ValidationErrors = ex.Errors
                }));

                return;
            }
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception is ValidationException)
            {
                var ex = actionExecutedContext.Exception as ValidationException;

                actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                actionExecutedContext.Response.Content = new StringContent(Json.Encode(new
                {
                    GeneralError = "Request didn't passed validation",
                    ValidationErrors = ex.Errors
                }));

                return Task.CompletedTask;
            }

            actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            actionExecutedContext.Response.Content = new StringContent(Json.Encode(new
            {
                GeneralError = "Please contact support",
            }));

            return Task.CompletedTask;
        }
    }
}
