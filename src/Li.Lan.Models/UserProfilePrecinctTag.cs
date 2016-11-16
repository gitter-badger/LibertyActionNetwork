using System;

namespace Li.Lan.Models
{
    public class UserProfilePrecinctTag
    {
        public int UserProfilePrecinctTagId { get; set; }

        public int InsertedBy { get; set; }

        public DateTime InsertedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public byte ActiveStatus { get; set; }

        public int UserId { get; set; }

        public int PrecinctTagId { get; set; }
    }
}