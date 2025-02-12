using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;
using System.Resources;

namespace Domain.Helpers
{
    public class Response<T> : IResponse<T>
    {
        private readonly ILogger _logger;
        private readonly List<Error> _errorList = new List<Error>();
        private readonly ResourceManager _resourceManager;

        public T Content { get; set; }
        public IEnumerable<Error> Errors => _errorList.AsReadOnly();
        public bool HasError => _errorList.Any();

        public Response() 
        {
            Content = default(T);
            _resourceManager = new ResourceManager("Domain.Resources.Resources", Assembly.GetExecutingAssembly());
            //_logger = loggerFactory.CreateLogger<Response<T>>();
        }

        public Response(T content)
        {
            Content = content;
            _resourceManager = new ResourceManager("Domain.Resources.Resources", Assembly.GetExecutingAssembly());
        }

        public Response<T> AddError(string errorName, HttpStatusCode httpStatusCode, string callerMemberName = "", bool printError = true, string[] arguments = null)
        {
            string errorMessage = _resourceManager.GetString(errorName);
            Error error = new Error(errorName, errorMessage, httpStatusCode, callerMemberName, arguments);
            _errorList.Add(error);

            //if (printError)
            //    _logger.LogError(error.ToString(), TraceEventType.Critical);

            return this;
        }

        public Response<T> AddErrors<TK>(Response<TK> response)
        {
            if (response != null)
            {
                _errorList.AddRange(response.Errors);
            }

            return this;
        }

        public ActionResult CreateHttpResponse(HttpStatusCode successHttpStatusCode = HttpStatusCode.OK)
        {
            if (HasError)
            {
                Error error = Errors.FirstOrDefault();
                ResponseError responseError = new ResponseError(error);

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
