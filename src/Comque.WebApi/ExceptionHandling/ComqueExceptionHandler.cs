using Comque.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Comque.WebApi.ExceptionHandling
{
    public class ComqueExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exceptionContext = context.ExceptionContext;
            if (exceptionContext == null)
            {
                return;
            }
            var exception = exceptionContext.Exception;
            if (exception == null)
            {
                return;
            }
            var request = context.ExceptionContext.Request;
            if (exception is ForbiddenException)
            {
                context.Result = new PlainTextActionResult(HttpStatusCode.Forbidden, exception.Message, request);
                return;
            }
            if (exception is NotFoundException)
            {
                context.Result = new PlainTextActionResult(HttpStatusCode.NotFound, exception.Message, request);
                return;
            }
            if (exception is InvalidInputException || exception is System.ComponentModel.DataAnnotations.ValidationException)
            {
                context.Result = new PlainTextActionResult(HttpStatusCode.BadRequest, exception.Message, request);
                return;
            }
            if (exception is InvalidOutputException)
            {
                context.Result = new PlainTextActionResult(HttpStatusCode.InternalServerError, exception.Message, request);
                return;
            }
            context.Result = new ResponseMessageResult(request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception));
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            var isTopLevel = base.ShouldHandle(context);
            return true;
        }
    }
}