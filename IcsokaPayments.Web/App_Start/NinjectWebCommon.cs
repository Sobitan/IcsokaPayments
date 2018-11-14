using System.Collections.Generic;
using IcsokaPayments.Data;
using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Data.Repository;
using IcsokaPayments.Domain.Entities;
using IcsokaPayments.Service;
using IcsokaPayments.Service.Caching;
using IcsokaPayments.Service.Configurations;
using IcsokaPayments.Service.Helpers;
using IcsokaPayments.Service.Infrastructure;
using IcsokaPayments.Service.Logging;
using IcsokaPayments.Service.Payments;
using IcsokaPayments.Web.Core.Processors;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(IcsokaPayments.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(IcsokaPayments.Web.App_Start.NinjectWebCommon), "Stop")]

namespace IcsokaPayments.Web.App_Start
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

            kernel.Bind<ISettlementProcessingService>().To<SettlementProcessingService>();

            kernel.Bind<IPaymentService>().To<PaymentService>();

            

            kernel.Bind<IPaymentMethod>().To<FlutterWave>().Named("FlutterWave");



            kernel.Bind<IPaymentMethodProvider>().To<PaymentMethodProvider>()

               .WithConstructorArgument("factories", new Dictionary<string, Func<IPaymentMethod>>
                      {
                          {"FlutterWave", () => kernel.Get<IPaymentMethod>("FlutterWave")},
                      });

            //Settings 
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<ISettingService>().To<SettingService>();

            kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InRequestScope();
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InRequestScope();

            //Authentication
            kernel.Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication).InRequestScope();
           // kernel.Bind<ApplicationUserManager>().ToMethod(t => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()).InRequestScope();
            //kernel.Bind<ApplicationSignInManager>().ToMethod(t => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>()).InRequestScope();
            //kernel.Bind<ApplicationRoleManager>().ToMethod(t => HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>()).InRequestScope();

            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<ICacheManager>().To<MemoryCacheManager>();
            kernel.Bind<IDateTimeHelper>().To<DateTimeHelper>();

            kernel.Bind<ILogger>().To<DefaultLogger>();
            kernel.Bind<IWebHelper>().To<WebHelper>();
            //Data
            kernel.Bind(typeof(IRepositoryServiceGet<>)).To(typeof(RepositoryService<>)).InRequestScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(RepositoryBase<>)).InRequestScope();
            kernel.Bind(typeof(IdentityDbContext<>)).To<IscokaPaymentEntities>().InRequestScope();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            kernel.Bind<PaymentSettings>().ToMethod(x => x.Kernel.Get<ISettingService>().LoadSetting<PaymentSettings>());
            kernel.Bind<FlutterWaveSetting>().ToMethod(x => x.Kernel.Get<ISettingService>().LoadSetting<FlutterWaveSetting>());

            kernel.Bind<ISettlementRepository>().To<SettlementRepository>();
            kernel.Bind<IRepositoryService<Settlement>>().To<SettlementService>();
        }        
    }
}
