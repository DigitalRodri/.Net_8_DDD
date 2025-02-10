namespace ApplicationCore.Helpers
{
    ///<Summary>
    /// Helper class for logging
    ///</Summary>
    public static class LoggerHelper
    {
        ///<Summary>
        /// Gets the InternalServerError error message
        ///</Summary>
        public static string GetInternalServerErrorMessage()
        {
            return "Exception ocurred. Please check logs for more information.";
        }

        ///<Summary>
        /// Gets the BadRequest error message
        ///</Summary>
        public static string GetBadRequestErrorMessage()
        {
            return " Bad Request. Please the input request.";
        }
    }
}
