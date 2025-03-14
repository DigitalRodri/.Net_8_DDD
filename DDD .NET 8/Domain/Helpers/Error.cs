using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Domain.Helpers
{
    public class Error(
        string errorName,
        string errorMessage,
        HttpStatusCode httpStatusCode,
        string callerFunction,
        string[] arguments)
    {
        public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
        public string ErrorName { get; set; } = errorName;
        public string ErrorMessage { get; set; } = arguments.IsNullOrEmpty() ? errorMessage : string.Format(errorMessage, arguments);
        public string CallerFunction { get; set; } = callerFunction;

        public override string ToString()
        {
            return arguments.IsNullOrEmpty() ? $"HttpStatusCode: {HttpStatusCode} -- {ErrorName}: {ErrorMessage}" : $"HttpStatusCode: {HttpStatusCode} -- {ErrorName} in {CallerFunction}: {ErrorMessage}";
        }
    }
}
