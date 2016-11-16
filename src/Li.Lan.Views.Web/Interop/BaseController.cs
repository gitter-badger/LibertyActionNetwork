using Li.Lan.Common.Models;
using Li.Lan.Common.Services;
using System;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Interop
{
    public class BaseController : Controller
    {
        public BaseController(
            IApplicationContextProvider applicationContextProvider,
            ILoggingService loggingService)
        {
            this.ApplicationContextProvider = applicationContextProvider;
            this.LoggingService = loggingService;
        }

        private IApplicationContextProvider ApplicationContextProvider { get; set; }

        protected ILoggingService LoggingService { get; set; }

        protected ApplicationContext ApplicationContext { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                // set up ApplicationContext
                this.ApplicationContext = this.ApplicationContextProvider.GetApplicationContext();
                
                // assign a new Id for this Request
                this.ApplicationContext.ActionId = Guid.NewGuid();

                ViewBag.IsAdmin = this.ApplicationContext.IsAdmin;

                // check if the referrer exists, and log if it is not our own
                if (Request.UrlReferrer != null
                    && Request.UrlReferrer.Host != Request.Url.Host)
                {
                    var controllerName = filterContext.RouteData.Values["controller"];
                    var actionName = filterContext.RouteData.Values["action"];

                    this.LoggingService.Log(String.Format("Referrer:{0} - Uri:{1}", Request.UrlReferrer, Request.Url), actionName.ToString(), controllerName.ToString());
                }
            }
            catch { /* no-op */}

            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var controllerName = filterContext.RouteData.Values["controller"];
                var actionName = filterContext.RouteData.Values["action"];

                this.LoggingService.Log(String.Format("Exception:{0}", filterContext.Exception.ToString()), actionName.ToString(), controllerName.ToString(), 1);

                var v = filterContext.Result as ViewResult;
                v.ViewBag.ActionId = this.ApplicationContext.ActionId;
            }
            catch { /* no-op */ }

            base.OnException(filterContext);
        }
    }
}