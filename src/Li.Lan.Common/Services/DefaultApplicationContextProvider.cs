using Li.Lan.Common.Models;

namespace Li.Lan.Common.Services
{
    public class DefaultApplicationContextProvider : IApplicationContextProvider
    {
        public DefaultApplicationContextProvider(ApplicationContext applicationContext)
        {
            this.ApplicationContext = applicationContext;
        }

        private ApplicationContext ApplicationContext { get; set; }

        public ApplicationContext GetApplicationContext()
        {
            return this.ApplicationContext;
        }
    }
}