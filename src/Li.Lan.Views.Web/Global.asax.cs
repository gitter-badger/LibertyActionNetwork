using Li.Lan.Common.Data;
using Li.Lan.Common.Services;
using Li.Lan.Data;
using Li.Lan.Views.Web.Interop;
using Li.Lan.Views.Web.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Li.Lan.Views.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected void Application_Start()
        {
            ControllerBuilder.Current.SetControllerFactory(new StaticControllerFactory());

            GlobalFilters.Filters.Add(new RequireHttpsAttribute());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Ensure ASP.NET Simple Membership is initialized only once on app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            this.AddKeepAliveCache();
        }

        protected void Application_Error()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var connectionStringProvider = new ConnectionStringProvider(connectionString);
                var applicationContextProvider = new ApplicationContextProvider();
                var loggingRepository = new LoggingRepository(connectionStringProvider);
                var commonService = new CommonService(applicationContextProvider);
                var loggingService = new LoggingService(loggingRepository, commonService, "Application", "Error", 1);

                foreach (var ex in HttpContext.Current.AllErrors)
                {
                    try
                    {
                        loggingService.Log(String.Format("Exception:{0}", ex.ToString()));
                    }
                    catch { /* no-op */ }
                }
            }
            catch { /* no-op */ }
        }

        private void AddKeepAliveCache()
        {
            HttpRuntime.Cache.Add("keepalive", "keepalive", null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, OnKeepAliveRemove);
        }

        public void OnKeepAliveRemove(string key, object value, CacheItemRemovedReason reason)
        {
            // add keepalive back into cache
            this.AddKeepAliveCache();

            // ping the Liberty Iowa site
            this.PingUri("http://www.libertyiowa.com/");
        }

        private void PingUri(string uri)
        {
            try
            {
                var request = WebRequest.Create(uri);
                request.GetResponse();
            }
            catch { /* no-op */ }
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}