using System;

namespace Li.Lan.Common.Models
{
    public partial class ApplicationContext
    {
        /// <summary>
        /// Gets or sets the current version of the running application.
        /// </summary>
        public string ApplicationVersion { get; set; }
        
        /// <summary>
        /// Gets or sets the UserId of the current user that is in context.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the IP Address of the Request.
        /// </summary>
        public string UserHostAddress { get; set; }

        /// <summary>
        /// Gets or sets a Guid that uniquely identifies the current Session.
        /// (For an individual user, or an instance of the application running, etc.)
        /// </summary>
        public Guid? SessionId { get; set; }

        /// <summary>
        /// Gets or sets a Guid that uniquely identifies the current Action within the current Session.
        /// (Examples for use: When a service boundary is crossed update the current ActionId, everything that takes place during that Request/Response is a single "Action")
        /// </summary>
        public Guid? ActionId { get; set; }
        
        /// <summary>
        /// Gets or sets the Tag that will be appended to all logs.
        /// </summary>
        public string LogTag { get; set; }

        public bool IsAdmin { get; set; }
    }
}