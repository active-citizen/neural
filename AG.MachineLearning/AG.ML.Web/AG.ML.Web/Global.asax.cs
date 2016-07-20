using AG.ML.Web.App_Start;
using System.Web.Http;

namespace AG.ML.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
