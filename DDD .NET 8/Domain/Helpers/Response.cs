using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Reflection;
using System.Resources;
using ILogger = Serilog.ILogger;

namespace Domain.Helpers
{
    public class Response<T>
    {
        private readonly ILogger _logger = Log.ForContext<Response<T>>();

        public T Content { get; }
        public Error Error { get; }
        public bool HasError => Error != null;

        private Response(T content)
        {
            Content = content;
        }

        private Response(string errorName, string callerMemberName = "", bool printError = false, string[] arguments = null)
        {
            var resourceManager = new ResourceManager("Domain.Resources.Resources", Assembly.GetExecutingAssembly());

            var errorMessage = resourceManager.GetString(errorName);
            Error = new Error(errorName, errorMessage, HttpStatusCode.NoContent, callerMemberName, arguments);

            if (printError)
                _logger.Error(Error.ToString());
        }

        private Response(Error error)
        {
            Error = error;
        }

        public static Response<T> AddContent(T content) => new(content);

        public static implicit operator Response<T>(T content) => AddContent(content);

        public static Response<T> AddError(string errorName, string callerMemberName = "", bool printError = false, string[] arguments = null)
            => new(errorName, callerMemberName, printError, arguments);

        public static Response<T> AddError(Error error) => new(error);

        public ActionResult CreateHttpResponse(HttpStatusCode successHttpStatusCode = HttpStatusCode.OK)
        {
            if (HasError)
            {
                var responseError = new ResponseError(Error);

                return new ObjectResult(responseError)
                {
                    StatusCode = (int)Error.HttpStatusCode
                };
            }

            if (successHttpStatusCode == HttpStatusCode.NoContent)
                return new NoContentResult();

            return new ObjectResult(Content)
            {
                StatusCode = (int)successHttpStatusCode
            };
        }
    }
}
