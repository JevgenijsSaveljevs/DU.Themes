using AutoMapper;
using DU.Themes.Api;
using DU.Themes.Mappings;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DataTables.AspNet.WebApi2;

namespace DU.Themes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.RegisterDataTables();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AppProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }

        

       
    }
}
