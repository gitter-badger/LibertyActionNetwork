using Li.Lan.Common.Services;
using Li.Lan.Views.Web.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(
            IApplicationContextProvider applicationContextProvider,
            ILoggingService loggingService)
            : base(
                applicationContextProvider,
                loggingService)
        {
            // no-op
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}