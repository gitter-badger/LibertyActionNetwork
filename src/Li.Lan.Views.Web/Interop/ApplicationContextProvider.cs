using Li.Lan.Common.Models;
using Li.Lan.Common.Services;
using Li.Lan.Models;
using System;
using System.Web;
using System.Web.Security;

namespace Li.Lan.Views.Web.Interop
{
    public class ApplicationContextProvider : IApplicationContextProvider
    {
        private const string SessionKeyApplicationContext = "SessionKeyApplicationContext";

        public ApplicationContext GetApplicationContext()
        {
            return this.CreateOrRetrieveApplicationContext();
        }

        private ApplicationContext CreateOrRetrieveApplicationContext()
        {
            if (HttpContext.Current == null
                || HttpContext.Current.Session == null)
                return null;

            var applicationContext = (ApplicationContext)HttpContext.Current.Session[SessionKeyApplicationContext];

            if (applicationContext == null)
            {
                applicationContext = new ApplicationContext();
                applicationContext.ApplicationVersion = ReferenceDictionary.ApplicationVersion;
                applicationContext.SessionId = Guid.NewGuid();

                HttpContext.Current.Session[SessionKeyApplicationContext] = applicationContext;
            }

            if (HttpContext.Current.Request != null)
            {
                applicationContext.UserHostAddress = HttpContext.Current.Request.UserHostAddress;
            }

            // User may be null if applicationContext was created before user signed in
            // Check UserName and attempt to update if null
            if (String.IsNullOrWhiteSpace(applicationContext.UserName)
                && HttpContext.Current.Request != null
                && HttpContext.Current.Request.IsAuthenticated)
            {
                var user = System.Web.Security.Membership.GetUser();

                if (user != null)
                {
                    applicationContext.UserId = (int)user.ProviderUserKey;
                    applicationContext.UserName = user.UserName;

                    applicationContext.IsAdmin = Roles.IsUserInRole(ReferenceDictionary.RoleAdmin);
                }
            }

            return applicationContext;
        }
    }
}