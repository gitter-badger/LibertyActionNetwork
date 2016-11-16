using Li.Lan.Common.Services;
using Li.Lan.Views.Web.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(
            IApplicationContextProvider applicationContextProvider,
            ILoggingService loggingService)
            : base(
                applicationContextProvider,
                loggingService)
        {
            // no-op
        }

        public ActionResult Unknown()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}