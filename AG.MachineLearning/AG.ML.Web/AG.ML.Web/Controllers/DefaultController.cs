using System.Web;
using System.Web.Http;

namespace AG.ML.Web.Controllers
{
    public class DefaultController : ApiController
    {
        // GET: Default
        public void Get()
        {
            if (HttpContext.Current.Request.RawUrl != "/")
                HttpContext.Current.Response.Redirect("/");
            else
                HttpContext.Current.Server.Transfer("/index.html");
        }
    }
}