using System.Net;

namespace Domain.Helpers
{
    public class Error
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorName { get; set; }
        public string ErrorMessage { get; set; }
        public string CallerFunction { get; set; }

        public Error(string errorName, string errorMessage, HttpStatusCode httpStatusCode, string callerFunction, string[] arguments)
        {
            HttpStatusCode = httpStatusCode;
            ErrorMessage = string.Format(errorMessage, arguments);
            ErrorName = errorName;
            CallerFunction = callerFunction;
        }

        public override string ToString()
        {
            return $"HttpStatusCode: {HttpStatusCode} -- {ErrorName} in {CallerFunction} -- {ErrorMessage}";
        }
    }
}
