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
        private readonly List<Error> _errorList = [];

        public T Content { get; }
        public IEnumerable<Error> Errors => _errorList.AsReadOnly();
        public bool HasError => _errorList.Any();

        private Response(T content)
        {
            Content = content;
        }

        private Response(string errorName, HttpStatusCode httpStatusCode, string callerMemberName = "", bool printError = false, string[] arguments = null)
        {
            ResourceManager resourceManager = new ResourceManager("Domain.Resources.Resources", Assembly.GetExecutingAssembly());

            var errorMessage = resourceManager.GetString(errorName);
            var error = new Error(errorName, errorMessage, httpStatusCode, callerMemberName, arguments);
            _errorList.Add(error);

            if (printError)
                _logger.Error(error.ToString());

        }

        private Response(IEnumerable<Error> errors)
        {
            _errorList.AddRange(errors);
        }

        public static Response<T> AddContent(T content) => new(content);

        public static implicit operator Response<T>(T content) => AddContent(content);

        public static Response<T> AddError(string errorName, HttpStatusCode httpStatusCode, string callerMemberName = "", bool printError = false, string[] arguments = null) => new(errorName, httpStatusCode, callerMemberName, printError, arguments);

        public static Response<T> AddError(IEnumerable<Error> errors) => new(errors);

        public ActionResult CreateHttpResponse(HttpStatusCode successHttpStatusCode = HttpStatusCode.OK)
        {
            if (HasError)
            {
                var error = Errors.FirstOrDefault();
                var responseError = new ResponseError(error);

                return new ObjectResult(responseError)
                {
                    StatusCode = (int)error.HttpStatusCode
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
