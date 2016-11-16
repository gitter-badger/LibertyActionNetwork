using System;

namespace Li.Lan.Common.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public Guid LogGuid { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? StoredOnUtc { get; set; }

        public string ApplicationVersion { get; set; }
        public int? UserId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? ActionId { get; set; }
        public string Tag { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Message { get; set; }
        
        public byte LogType { get; set; }

        public string UserHostAddress { get; set; }
    }
}