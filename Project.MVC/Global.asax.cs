using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using AutoMapper;
using Project.MVC_.Mapping;

namespace Project.MVC_
{
    public class MvcApplication : HttpApplication
    {
        public static IMapper MapperInstance { get; private set; }

        protected void Application_Start()
        {
            // Set the EF initializer for seeding demo data
            System.Data.Entity.Database.SetInitializer(new Project.Service_.Data.VehicleDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MVCAutoMapperProfile>();
            });
            MapperInstance = config.CreateMapper();
        }
    }
}