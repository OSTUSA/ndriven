using System.Web.Http;
using Infrastructure.IoC.EntityFramework;
using Infrastructure.IoC.Migrations;
using Infrastructure.IoC.NHibernate;
using Presentation.Web.Services;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Presentation.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Presentation.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Presentation.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Comment this line if using Entity Framework instead of NHibernate
            kernel.Load(new NHibernateModule());
            // Uncomment this module to use Entity Framework instead of NHibernate
            //kernel.Load(new EntityFrameworkModule()); 

            kernel.Load(new MigrationsModule());
            kernel.Bind<IAuthenticationService>().To<AuthenticationService>();
        }        
    }
}
