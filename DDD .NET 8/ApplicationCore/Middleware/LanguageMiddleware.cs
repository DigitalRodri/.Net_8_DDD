using System.Globalization;

namespace ApplicationCore.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Sets the Accept-Language header language as Culture
        // en-US is the default language if none is provided explicitly
        public async Task Invoke(HttpContext context)
        {
            string userLanguage = context.Request.Headers["Accept-Language"].ToString();

            CultureInfo selectedLanguage = userLanguage?.Length > 0
                ? new CultureInfo(userLanguage)
                : CultureInfo.InvariantCulture;

            Thread.CurrentThread.CurrentCulture = selectedLanguage;
            Thread.CurrentThread.CurrentUICulture = selectedLanguage;

            await _next(context);
        }
    }
}
