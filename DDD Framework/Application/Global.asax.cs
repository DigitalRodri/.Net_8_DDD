using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Application
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start() 
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
        }

        protected void Application_BeginRequest()
        {
            // Select the first language out of the ones available and set that as Culture
            // Reads Accept-Language header
            string[] userLanguages = HttpContext.Current.Request.UserLanguages;

            CultureInfo selectedLanguage = userLanguages?.Length > 0
                ? new CultureInfo(userLanguages[0])
                : CultureInfo.InvariantCulture;

            Thread.CurrentThread.CurrentCulture = selectedLanguage;
            Thread.CurrentThread.CurrentUICulture = selectedLanguage;
        }
    }
}
