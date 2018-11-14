using System;
using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IcsokaPayments.Data;
using IcsokaPayments.Service.Logging;

namespace IcsokaPayments.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IscokaPaymentEntities, Data.Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {
                //log
                var logger = DependencyResolver.Current.GetService<ILogger>();
                logger.Information("Application started");
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}
