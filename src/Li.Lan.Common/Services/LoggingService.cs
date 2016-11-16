using Li.Lan.Common.Data;
using Li.Lan.Common.Models;

namespace Li.Lan.Common.Services
{
    public interface ILoggingService
    {
        Log CreateLog(string message);
        Log CreateLog(string message, byte logType);
        Log CreateLog(string message, string subCategory);
        Log CreateLog(string message, string subCategory, byte logType);
        Log CreateLog(string message, string subCategory, string category);
        Log CreateLog(string message, string subCategory, string category, byte logType);

        Log Log(string message);
        Log Log(string message, byte logType);
        Log Log(string message, string subCategory);
        Log Log(string message, string subCategory, byte logType);
        Log Log(string message, string subCategory, string category);
        Log Log(string message, string subCategory, string category, byte logType);

        Log Log(Log log);
    }

    public class LoggingService : ILoggingService
    {
        public LoggingService(
            ILoggingRepository loggingRepository,
            ICommonService commonService,
            string defaultCategory,
            string defaultSubCategory,
            byte defaultLogType)
        {
            this.LoggingRepository = loggingRepository;
            this.CommonService = commonService;

            this.DefaultCategory = defaultCategory;
            this.DefaultSubCategory = defaultSubCategory;
            this.DefaultLogType = defaultLogType;
        }

        private ILoggingRepository LoggingRepository { get; set; }

        private ICommonService CommonService { get; set; }

        private string DefaultCategory { get; set; }

        private string DefaultSubCategory { get; set; }

        private byte DefaultLogType { get; set; }

        /// <summary>
        /// Creates a log but does not Store it with the given Message
        /// and default LogType, Category, SubCategory
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log CreateLog(string message)
        {
            return this.CreateLog(message, this.DefaultLogType);
        }

        /// <summary>
        /// Creates a log but does not Store it with the given Message, LogType
        /// and default Category, SubCategory
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log CreateLog(string message, byte logType)
        {
            return this.CreateLog(message, this.DefaultSubCategory, logType);
        }

        /// <summary>
        /// Creates a log but does not Store it with the given Message, SubCategory
        /// and default LogType, Category
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log CreateLog(string message, string subCategory)
        {
            return this.CreateLog(message, subCategory, this.DefaultLogType);
        }

        /// <summary>
        /// Creates a log but does not Store it with the given Message, SubCategory, LogType
        /// and default Category
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log CreateLog(string message, string subCategory, byte logType)
        {
            return this.CreateLog(message, subCategory, this.DefaultCategory, logType);
        }

        /// <summary>
        /// Creates a log but does not Store it with the given Message, SubCategory, Category
        /// and default LogType
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log CreateLog(string message, string subCategory, string category)
        {
            return this.CreateLog(message, subCategory, category, this.DefaultLogType);
        }

        /// <summary>
        /// Creates a log but does not Store it with the given Message, SubCategory, Category, LogType
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log CreateLog(string message, string subCategory, string category, byte logType)
        {
            var log = this.CreateLog();

            log.Message = message;
            log.SubCategory = subCategory;
            log.Category = category;
            log.LogType = logType;

            return log;
        }

        /// <summary>
        /// Creates and Stores a Log with the given Message
        /// and default LogType, Category, SubCategory
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log Log(string message)
        {
            return this.Log(message, this.DefaultLogType);
        }

        /// <summary>
        /// Creates and Stores a Log with the given Message, LogType
        /// and default Category, SubCategory
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log Log(string message, byte logType)
        {
            return this.Log(message, this.DefaultSubCategory, logType);
        }

        /// <summary>
        /// Creates and Stores a Log with the given Message, SubCategory
        /// and default LogType, Category
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log Log(string message, string subCategory)
        {
            return this.Log(message, subCategory, this.DefaultLogType);
        }

        /// <summary>
        /// Creates and Stores a Log with the given Message, SubCategory, LogType
        /// and default Category
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log Log(string message, string subCategory, byte logType)
        {
            return this.Log(message, subCategory, this.DefaultCategory, logType);
        }

        /// <summary>
        /// Creates and Stores a Log with the given Message, SubCategory, Category
        /// and default LogType
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Log Log(string message, string subCategory, string category)
        {
            return this.Log(message, subCategory, category, this.DefaultLogType);
        }

        /// <summary>
        /// Creates and Stores a Log with the given Message SubCategory, Category, LogType
        /// </summary>
        /// <param name="message">The Message to be logged.</param>
        /// <param name="category">A Category for the log (optional. will use default if not provided).</param>
        /// <param name="subCategory">A SubCategory for the log (optional. will use default if not provided).</param>
        /// <param name="logType">The type of log.</param>
        /// <returns>The Log created.</returns>
        public Log Log(string message, string subCategory, string category, byte logType)
        {
            var log = this.CreateLog(message, subCategory, category, logType);

            this.Log(log);

            return log;
        }

        public Log Log(Log log)
        {
            return this.LoggingRepository.StoreLog(log);
        }

        private Log CreateLog()
        {
            var log = new Log();

            // set Guid here, data store will assign "LogId" as needed
            log.LogGuid = this.CommonService.CreateGuid();
            log.CreatedOnUtc = this.CommonService.GetCurrentDateTimeUtc();

            // set defaults
            log.Category = this.DefaultCategory;
            log.SubCategory = this.DefaultSubCategory;
            log.LogType = this.DefaultLogType;

            // set application context values
            var ac = this.GetApplicationContext();

            log.ApplicationVersion = ac.ApplicationVersion;
            log.UserId = ac.UserId;
            log.UserHostAddress = ac.UserHostAddress;
            log.SessionId = ac.SessionId;
            log.ActionId = ac.ActionId;
            log.Tag = ac.LogTag;

            return log;
        }

        private ApplicationContext GetApplicationContext()
        {
            return this.CommonService.GetApplicationContext();
        }
    }
}