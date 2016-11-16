using Li.Lan.Common.Models;

namespace Li.Lan.Common.Services
{
    public interface IApplicationContextProvider
    {
        ApplicationContext GetApplicationContext();
    }
}