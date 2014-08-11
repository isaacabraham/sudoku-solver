using Lydian.SodokuSolver.Website;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace Lydian.SodokuSolver.Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            app.UseStaticFiles("/Web");
            app.UseWebApi(config);
        }
    }

    public class SodokuController : ApiController
    {
        public string Get()
        {
            return "Foo";
        }
    }
}
