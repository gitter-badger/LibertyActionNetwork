using Li.Lan.Common.Data;
using Li.Lan.Common.Services;
using Li.Lan.Data;
using Li.Lan.Views.Web.Controllers;
using Li.Lan.Views.Web.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Li.Lan.Views.Web.Interop
{
    public class StaticControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var connectionStringProvider = new ConnectionStringProvider(connectionString);
            var applicationContextProvider = new ApplicationContextProvider();
            var loggingRepository = new LoggingRepository(connectionStringProvider);
            var commonService = new CommonService(applicationContextProvider);
            var loggingService = new LoggingService(loggingRepository, commonService, null, null, 0);
            var voterRepository = new VoterRepository(connectionStringProvider);

            var dimensionRepository = new DimensionRespository(connectionStringProvider);
            var httpContext = HttpContext.Current;
            var referenceDictionaryProvider = new ReferenceDictionaryProvider(dimensionRepository, httpContext);

            switch (controllerName)
            {
                case "Voter":

                    return new VoterController(
                        applicationContextProvider,
                        referenceDictionaryProvider,
                        loggingService,
                        commonService,
                        voterRepository);

                case "Home":
                    return new HomeController(
                        applicationContextProvider,
                        loggingService);

                case "Account":
                    
                    var sendEmailActive = Settings.Default.SendEmailActive;
                    var siteRootUri = Settings.Default.SiteRootUri;
                    var smtpHostUri = Settings.Default.SmtpHostUri;
                    var smtpHostPort = Settings.Default.SmtpHostPort;
                    var smtpUserName = Settings.Default.SmtpUserName;
                    var smtpPassword = Settings.Default.SmtpPassword;
                    var fromAddress = Settings.Default.FromAddress;

                    var mailService = new MailService(sendEmailActive, siteRootUri, smtpHostUri, smtpHostPort, smtpUserName, smtpPassword, fromAddress);

                    return new AccountController(
                        applicationContextProvider,
                        loggingService,
                        mailService);

                case "Error":
                    return new ErrorController(
                        applicationContextProvider,
                        loggingService);

                case "Admin":

                    var userRepository = new UserRepository(connectionStringProvider);

                    return new AdminController(
                        applicationContextProvider,
                        loggingService,
                        referenceDictionaryProvider,
                        userRepository);
            }

            return base.CreateController(requestContext, controllerName);
        }
    }
}