using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace System.Web.Http
{
    public class PlainTextActionResult : IHttpActionResult
    {
        private readonly HttpStatusCode statusCode;
        private readonly string message;
        private readonly HttpRequestMessage request;

        public PlainTextActionResult(HttpStatusCode statusCode, string message, HttpRequestMessage request)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            this.statusCode = statusCode;
            this.message = message;
            this.request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(statusCode);
            try
            {
                response.Content = new StringContent(message);
                response.RequestMessage = request;
            }
            catch
            {
                response.Dispose();
                throw;
            }
            return response;

            //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
            //response.Content = new StringContent(Message); // Put the message in the response body (text/plain content).
            //response.RequestMessage = Request;
            //return response;
        }
    }

    public static class ApiControllerExtensions
    {
        public static PlainTextActionResult NotFound(this ApiController controller, string message)
        {
            return new PlainTextActionResult(HttpStatusCode.NotFound, message, controller.Request);
        }

        public static PlainTextActionResult Forbidden(this ApiController controller, string message)
        {
            return new PlainTextActionResult(HttpStatusCode.Forbidden, message, controller.Request);
        }

        public static PlainTextActionResult Conflict(this ApiController controller, string message)
        {
            return new PlainTextActionResult(HttpStatusCode.Conflict, message, controller.Request);
        }

        public static PlainTextActionResult InternalServerError(this ApiController controller, string message)
        {
            return new PlainTextActionResult(HttpStatusCode.InternalServerError, message, controller.Request);
        }

        public static PlainTextActionResult StatusCode(this ApiController controller, HttpStatusCode statusCode, string message)
        {
            return new PlainTextActionResult(statusCode, message, controller.Request);
        }

        public static IHttpActionResult ComqueError(this ApiController controller, Comque.Result result)
        {
            if (result.Status == Comque.ResultStatus.Forbidden)
            {
                return controller.Forbidden(result.Message);
            }
            if (result.Status == Comque.ResultStatus.NotFound)
            {
                return controller.NotFound(result.Message);
            }
            if (result.Status == Comque.ResultStatus.Error)
            {
                return controller.InternalServerError(result.Message);
            }
            if (result.Status == Comque.ResultStatus.InvalidInput || result.Status == Comque.ResultStatus.InvalidOutput)
            {
                return new BadRequestErrorMessageResult(result.Message, controller);
            }
            if (result.Status == Comque.ResultStatus.Success)
            {
                return null;
            }
            return new InternalServerErrorResult(controller);
        }
    }
}