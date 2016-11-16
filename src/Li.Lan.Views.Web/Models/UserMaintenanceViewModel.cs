using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Models
{
    public class UserMaintenanceViewModel
    {
        public UserMaintenanceViewModel()
        {
            this.UserProfileSearchCriteriaViewModel = new UserProfileSearchCriteriaViewModel();
        }

        public UserProfileSearchCriteriaViewModel UserProfileSearchCriteriaViewModel { get; set; }

        public List<UserViewModel> Users { get; set; }

        public List<SelectListItem> Precincts { get; set; }

        public List<SelectListItem> AllRoles { get; set; }
    }
}