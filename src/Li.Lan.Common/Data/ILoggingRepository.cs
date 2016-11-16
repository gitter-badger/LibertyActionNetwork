using Li.Lan.Common.Models;

namespace Li.Lan.Common.Data
{
    public interface ILoggingRepository
    {
        Log StoreLog(Log log);
    }
}