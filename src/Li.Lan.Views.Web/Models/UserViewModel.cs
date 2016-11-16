using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Li.Lan.Views.Web.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            this.PrecinctTags = new List<PrecinctTagViewModel>();
        }

        public int UserId { get; set; }

        [Required]
        [StringLength(250)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public List<PrecinctTagViewModel> PrecinctTags { get; set; }
    }
}