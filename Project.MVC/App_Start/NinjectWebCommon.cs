using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Project.Service_.Interfaces;
using Project.Service_.Services;
using AutoMapper;
using Project.Service_.Mapping;            // Backend AutoMapper profile (DTO <-> Model)
using Project.MVC_.Mapping;               // MVC AutoMapper profile (DTO <-> ViewModel)

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Project.MVC_.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Project.MVC_.App_Start.NinjectWebCommon), "Stop")]

namespace Project.MVC_.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Bind services
            kernel.Bind<IVehicleMakeService>().To<VehicleMakeService>().InRequestScope();
            kernel.Bind<IVehicleModelService>().To<VehicleModelService>().InRequestScope();

            // Configure AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();           // DTO <-> Domain model (backend)
                cfg.AddProfile<MVCAutoMapperProfile>();         // DTO <-> ViewModel (MVC)
            });

            // Bind IMapper instance
            IMapper mapper = mapperConfig.CreateMapper();
            kernel.Bind<IMapper>().ToConstant(mapper);
        }
    }
}
