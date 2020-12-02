[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Tools.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Tools.App_Start.NinjectWebCommon), "Stop")]

namespace Tools.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Sihirdar.WebServiceV3.Provider.RiotApi;
    using Tools.Service.Interface;
    using Tools.Service;
    using Sihirdar.WebServiceV3.Provider.RiotApi.Interfaces;
    using System.Web.Configuration;

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
            //Riot Web Service
            kernel.Bind<RiotApiConfig>().ToMethod<RiotApiConfig>(context =>
            {
                var config = new RiotApiConfig
                {
                    ApiKey = WebConfigurationManager.AppSettings["RiotApiKey"],
                    RateLimitPer10M = Convert.ToInt32(WebConfigurationManager.AppSettings["RiotRateLimitPer10M"]),
                    RateLimitPer10S = Convert.ToInt32(WebConfigurationManager.AppSettings["RiotRateLimitPer10S"])
                };
                return config;
            });

            kernel.Bind<ICache>().To<Cache>().InSingletonScope();
            kernel.Bind<IRiotApi>().To<RiotApi>().InSingletonScope();
            //kernel.Bind<IStaticRiotApi>().To<StaticRiotApi>().InSingletonScope();
            //kernel.Bind<IStatusRiotApi>().To<StatusRiotApi>().InSingletonScope();
            //kernel.Bind<ITournamentRiotApi>().To<TournamentRiotApi>().InSingletonScope();

            //Local service
            kernel.Bind<IPlayerService>().To<PlayerService>().InSingletonScope();
            kernel.Bind<IStaticDataService>().To<StaticDataService>().InSingletonScope();
            kernel.Bind<IContentService>().To<ContentService>().InSingletonScope();
            kernel.Bind<IAdvertService>().To<AdvertService>().InSingletonScope();

            kernel.Bind<IAovChampService>().To<AovChampService>().InSingletonScope();
            kernel.Bind<IAovSkinService>().To<AovSkinService>().InSingletonScope();
            kernel.Bind<IAovSpellService>().To<AovSpellService>().InSingletonScope();
            kernel.Bind<IAovChampSkinAssgnService>().To<AovChampSkinAssgnService>().InSingletonScope();
            kernel.Bind<IAovChampSpellAssgnService>().To<AovChampSpellAssgnService>().InSingletonScope();

            //kernel.Bind<IStaticRiotApi>().ToMethod<StaticRiotApi>(context =>
            //{
            //    var dbContext = new StaticRiotApi();
            //    return dbContext;
            //}).InRequestScope();
        }
    }
}
