using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Li.Lan.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            this.Roles = new List<Role>();
            this.PrecinctTags = new List<PrecinctTag>();
        }

        [Key]
        public int UserId { get; set; }

        public Guid UserGuid { get; set; }

        public int InsertedBy { get; set; }

        public DateTime InsertedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public byte ActiveStatus { get; set; }

        public string UserName { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<PrecinctTag> PrecinctTags { get; set; }
    }
}