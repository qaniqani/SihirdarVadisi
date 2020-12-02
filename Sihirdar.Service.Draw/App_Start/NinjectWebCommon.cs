[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Sihirdar.Service.Draw.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Sihirdar.Service.Draw.App_Start.NinjectWebCommon), "Stop")]

namespace Sihirdar.Service.Draw.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Sihirdar.Service.Draw.Service;
    using Sihirdar.Service.Draw.Service.Interface;
    using Sihirdar.DataAccessLayer.Infrastructure;

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
            kernel.Bind<AdminDbContext>().ToMethod<AdminDbContext>(context =>
            {
                var dbContext = new AdminDbContext("AdminDbContext");
                return dbContext;
            }).InRequestScope();

            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            kernel.Bind<IMemberService>().To<MemberService>().InSingletonScope();
            kernel.Bind<IDefinitionService>().To<DefinitionService>().InSingletonScope();
        }
    }
}
