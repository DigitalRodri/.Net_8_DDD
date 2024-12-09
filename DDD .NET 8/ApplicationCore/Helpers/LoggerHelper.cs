namespace ApplicationCore.Helpers
{
    ///<Summary>
    /// Helper class for logging
    ///</Summary>
    public static class LoggerHelper
    {
        ///<Summary>
        /// Gets a string with the current date and time formatted
        ///</Summary>
        public static string GetCurrentDateTimeLog()
        {
            return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " --------- ";
        }
        ///<Summary>
        /// Gets the exception formatted with the current date and time
        ///</Summary>
        public static string GetExceptionLog(Exception ex)
        {
            return GetCurrentDateTimeLog() + ex.ToString();
        }
        ///<Summary>
        /// Gets the InternalServerError error message
        ///</Summary>
        public static string GetInternalServerErrorMessage()
        {
            return "Exception ocurred. Please check logs for more information";
        }
    }
}
