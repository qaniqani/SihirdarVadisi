[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AdminProject.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AdminProject.App_Start.NinjectWebCommon), "Stop")]

namespace AdminProject.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using AdminProject.Models;
    using System.Web.Configuration;
    using AdminProject.Services.Interface;
    using AdminProject.Services;
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
            //Website general settings
            kernel.Bind<RuntimeSettings>().ToMethod<RuntimeSettings>(context =>
            {
                var maxWitdh = WebConfigurationManager.AppSettings["ImageMaxWidth"];
                var maxHeight = WebConfigurationManager.AppSettings["ImageMaxHeight"];

                var contactAddress = WebConfigurationManager.AppSettings["ContactAddress"];
                var emailAddress = WebConfigurationManager.AppSettings["EmailAddress"];
                var emailPassword = WebConfigurationManager.AppSettings["EmailPassword"];
                var port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"]);
                var smtp = WebConfigurationManager.AppSettings["Smtp"];
                var domain = WebConfigurationManager.AppSettings["Domain"];

                var setting = new RuntimeSettings
                {
                    ContactAddress = contactAddress,
                    EmailAddress = emailAddress,
                    EmailPassword = emailPassword,
                    Port = port,
                    Smtp = smtp,
                    Domain = domain,

                    ImageMaxHeight = Convert.ToInt32(maxHeight),
                    ImageMaxWidth = Convert.ToInt32(maxWitdh),

                    Language = "tr",
                    LanguageId = 1,
                    PictureExtensionTypes = new[] { ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".BMP", ".JPG", ".JPEG", ".PNG", ".GIF" },
                    PictureMimeType = new[] { "image/jpeg", "image/pjpeg", "image/bmp", "image/x-icon", "image/png", "image/gif" },

                    UserExtensionTypes = new[] { ".bmp", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".pdf" },
                    UserMimeTypes = new[] { "image/jpeg", "image/pjpeg", "image/bmp", "image/png", "image/gif", "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },

                    FileMimeTypes = new[]
                    {
                        "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/vnd.openxmlformats-officedocument.wordprocessingml.template",
                        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.openxmlformats-officedocument.spreadsheetml.template",
                        "application/vnd.ms-excel.sheet.macroEnabled.12", "application/vnd.ms-excel.template.macroEnabled.12", "application/vnd.ms-excel.addin.macroEnabled.12",
                        "application/vnd.ms-excel.sheet.binary.macroEnabled.12", "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                        "application/vnd.openxmlformats-officedocument.presentationml.slideshow", "application/vnd.openxmlformats-officedocument.presentationml.template", "application/vnd.ms-powerpoint.presentation.macroEnabled.12", "application/x-7z-compressed", "application/pdf", "application/vnd.android.package-archive", "image/vnd.dxf",
                        "model/vnd.dwf", "image/bmp", "text/csv", "image/vnd.dwg", "image/gif", "video/h263", "image/x-icon", "image/jpeg", "application/x-msaccess","application/vnd.ms-xpsdocument",
                        "video/mpeg", "audio/mp4", "video/mp4", "application/mp4", "application/ogg", "audio/ogg", "video/ogg", "audio/webm", "video/webm", "application/vnd.oasis.opendocument.text",
                        "application/vnd.oasis.opendocument.database", "application/vnd.oasis.opendocument.graphics", "application/vnd.oasis.opendocument.text-master", "application/x-font-otf",
                        "image/vnd.adobe.photoshop", "application/rtf", "text/richtext", "text/x-vcard", "application/xml", "application/xslt+xml", "application/x-rar-compressed", "application/zip",
                        "image/png", "video/3gpp", "video/x-msvideo", "application/x-shockwave-flash", "application/onenote", "application/vnd.ms-powerpoint.addin.macroenabled.12",
                        "application/vnd.ms-powerpoint.slideshow.macroenabled.12", "video/x-ms-wm", "audio/x-ms-wma", "audio/mpeg", "audio/webm", "video/webm"
                    },
                    FileExtensionTypes = new[] { ".doc", ".docx", ".xls", ".xlsx", ".rar", ".zip", ".7z", ".bmp", ".jpg",
                        ".jpeg", ".ico", ".png", ".3gp", ".avi", ".pdf", ".swf", ".apk", ".dxf",
                        ".dwg", ".gif", ".h263", ".mpg", ".mpeg", ".mdb", ".xlam", ".xlsb", ".xltm",
                        ".xlsm", ".pptx", ".ppt", ".dotx", ".onetoc", ".ppam", ".pptm", ".ppsm", ".wm",
                        ".wma", ".xps", ".mpga", ".mp4", ".mp4a", ".ogv", ".oga", ".webm", ".weba",".odb",
                        ".odg", ".odt", ".odm", ".otf", ".psd", ".rtf", ".rtx", ".vcf", ".xml", ".xslt" }
                };
                return setting;
            }).InSingletonScope();

            //Website Service
            kernel.Bind<ICategoryService>().To<CategoryService>().InSingletonScope();
            kernel.Bind<IPictureSizeService>().To<PictureSizeService>().InSingletonScope();
            kernel.Bind<IPromiseDayService>().To<PromiseDayService>().InSingletonScope();
            kernel.Bind<IEmailService>().To<EmailService>().InSingletonScope();
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            kernel.Bind<ILanguageService>().To<LanguageService>().InSingletonScope();
            kernel.Bind<ISurveyService>().To<SurveyService>().InSingletonScope();
            kernel.Bind<IAnswerService>().To<AnswerService>().InSingletonScope();
            kernel.Bind<IAdvertService>().To<AdvertService>().InSingletonScope();
            kernel.Bind<IContentService>().To<ContentService>().InSingletonScope();
            kernel.Bind<IPictureService>().To<PictureService>().InSingletonScope();
            kernel.Bind<IEsportCalendarService>().To<EsportCalendarService>().InSingletonScope();
            kernel.Bind<ISliderService>().To<SliderService>().InSingletonScope();
            kernel.Bind<ILiveBroadcastService>().To<LiveBroadcastService>().InSingletonScope();
            kernel.Bind<ICounterService>().To<CounterService>().InSingletonScope();
            kernel.Bind<IVideoEmbedService>().To<VideoEmbedService>().InSingletonScope();
            kernel.Bind<IGalleryService>().To<GalleryService>().InSingletonScope();
            kernel.Bind<IAdminService>().To<AdminService>().InSingletonScope();
            kernel.Bind<IThirtPartService>().To<ThirtPartService>().InSingletonScope();
            kernel.Bind<IRssService>().To<RssService>().InSingletonScope();
            kernel.Bind<IArenaValorChampService>().To<ArenaValorChampService>().InSingletonScope();

            kernel.Bind<IAovChampService>().To<AovChampService>().InSingletonScope();
            kernel.Bind<IAovSkinService>().To<AovSkinService>().InSingletonScope();
            kernel.Bind<IAovSpellService>().To<AovSpellService>().InSingletonScope();
            kernel.Bind<IAovChampSkinAssgnService>().To<AovChampSkinAssgnService>().InSingletonScope();
            kernel.Bind<IAovChampSpellAssgnService>().To<AovChampSpellAssgnService>().InSingletonScope();

            //Riot Web Service
            //kernel.Bind<RiotApiConfig>().ToMethod<RiotApiConfig>(context =>
            //{
            //    var config = new RiotApiConfig
            //    {
            //        ApiKey = WebConfigurationManager.AppSettings["RiotApiKey"],
            //        RateLimitPer10M = Convert.ToInt32(WebConfigurationManager.AppSettings["RiotRateLimitPer10M"]),
            //        RateLimitPer10S = Convert.ToInt32(WebConfigurationManager.AppSettings["RiotRateLimitPer10S"])
            //    };
            //    return config;
            //});

            //kernel.Bind<IRiotApi>().To<RiotApi>().InSingletonScope();
            //kernel.Bind<IStaticRiotApi>().To<StaticRiotApi>().InSingletonScope();
            //kernel.Bind<IStatusRiotApi>().To<StatusRiotApi>().InSingletonScope();
            //kernel.Bind<ITournamentRiotApi>().To<TournamentRiotApi>().InSingletonScope();

            //kernel.Bind<IRiotService>().To<RiotService>().InSingletonScope();

            kernel.Bind<AdminDbContext>().ToMethod<AdminDbContext>(context =>
            {
                var dbContext = new AdminDbContext("AdminDbContext");
                return dbContext;
            }).InRequestScope();
        }
    }
}
