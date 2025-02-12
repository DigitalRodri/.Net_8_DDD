namespace Domain.Helpers
{
    public class ResponseError
    {
        public string ErrorName { get; set; }
        public string Message { get; set; }

        public ResponseError() { }

        public ResponseError(Error error) 
        {
            ErrorName = error.ErrorName;
            Message = error.ErrorMessage;
        }
    }
}
