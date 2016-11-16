using Li.Lan.Common.Services;
using Li.Lan.Data;
using Li.Lan.Models;
using Li.Lan.Views.Web.Interop;
using Li.Lan.Views.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Li.Lan.Views.Web.Controllers
{
    [Authorize(Roles = ReferenceDictionary.RoleAdmin)]
    public class AdminController : BaseController
    {
        public AdminController(
            IApplicationContextProvider applicationContextProvider,
            ILoggingService loggingService,
            IReferenceDictionaryProvider referenceDictionaryProvider,
            IUserRepository userRepository)
            : base(
                applicationContextProvider,
                loggingService)
        {
            this.ReferenceDictionaryProvider = referenceDictionaryProvider;
            this.UserRepository = userRepository;
        }

        private IReferenceDictionaryProvider ReferenceDictionaryProvider { get; set; }

        private IUserRepository UserRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserMaintenance()
        {
            var vm = new UserMaintenanceViewModel();

            var sc = new UserProfileSearchCriteria();

            var users = this.UserRepository.SelectUserProfiles(sc);

            vm.Users = users.Select(x => Mapper.Map(x)).ToList();

            vm.AllRoles = this.GetAllRolesSelectListWithAny();

            return View(vm);
        }

        [HttpPost]
        public ActionResult UserMaintenance(UserProfileSearchCriteriaViewModel userProfileSearchCriteriaViewModel)
        {
            var vm = new UserMaintenanceViewModel();
            vm.UserProfileSearchCriteriaViewModel = userProfileSearchCriteriaViewModel;

            var sc = new UserProfileSearchCriteria();

            sc.UserName = userProfileSearchCriteriaViewModel.UserName;
            sc.RoleName = userProfileSearchCriteriaViewModel.RoleName;
            sc.ResultLimit = userProfileSearchCriteriaViewModel.ResultLimit;

            var userProfiles = this.UserRepository.SelectUserProfiles(sc);
            vm.Users = userProfiles.Select(x => Mapper.Map(x)).ToList();

            vm.AllRoles = this.GetAllRolesSelectListWithAny();

            return View(vm);
        }

        public ActionResult UserAdd()
        {
            var vm = new UserAddViewModel();

            vm.AllRoles = this.GetAllRolesSelectListWithBlank();

            return View(vm);
        }

        [HttpPost]
        public ActionResult UserAdd(UserAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(vm.UserName, vm.Password);

                    foreach (var roleName in vm.Roles)
                    {
                        if (Roles.RoleExists(roleName))
                            Roles.AddUserToRole(vm.UserName, roleName);
                    }

                    return RedirectToAction("UserMaintenance");
                }
                catch (Exception ex)
                {
                    this.LoggingService.Log(String.Format("Exception: {0}", ex.ToString()), "UserAdd", "Admin", 1);

                    this.ModelState.AddModelError("", "There was an issue while trying to create the user.");
                }
            }

            vm.AllRoles = this.GetAllRolesSelectListWithBlank();

            return View(vm);
        }

        public ActionResult UserEdit(string userName)
        {
            var userProfiles = this.UserRepository.SelectUserProfiles(new UserProfileSearchCriteria() { UserName = userName });

            if (userProfiles != null
                && userProfiles.Count() == 1)
            {
                var userProfile = userProfiles.First();

                var vm = new UserEditViewModel();

                vm.UserId = userProfile.UserId;
                vm.UserName = userProfile.UserName;
                vm.Roles = userProfile.Roles.Select(x => x.RoleName).ToList();

                vm.AllRoles = this.GetAllRolesSelectListWithBlank();

                return View(vm);
            }

            // user with given userName does not exist, redirect to list
            return RedirectToAction("UserMaintenance");
        }

        [HttpPost]
        public ActionResult UserEdit(UserEditViewModel vm)
        {
            if (vm == null || !WebSecurity.UserExists(vm.UserName))
                return RedirectToAction("UserMaintenance");

            try
            {
                // ensure vm.Roles is not null
                vm.Roles = vm.Roles ?? new List<string>();

                var currentRoles = Roles.GetRolesForUser(vm.UserName);

                // ensure not null
                currentRoles = currentRoles ?? new string[0];

                // find roles to remove user from
                foreach (var removeRole in currentRoles.Except(vm.Roles))
                {
                    if (Roles.IsUserInRole(vm.UserName, removeRole))
                    {
                        Roles.RemoveUserFromRole(vm.UserName, removeRole);
                    }
                }

                // find new roles to add to user
                foreach (var addRole in vm.Roles.Except(currentRoles))
                {
                    if (Roles.RoleExists(addRole)
                        && !Roles.IsUserInRole(vm.UserName, addRole))
                    {
                        Roles.AddUserToRole(vm.UserName, addRole);
                    }
                }

                // if reset password provided, reset the users password
                if (!String.IsNullOrWhiteSpace(vm.ResetPassword))
                {
                    var token = WebSecurity.GeneratePasswordResetToken(vm.UserName);
                    WebSecurity.ResetPassword(token, vm.ResetPassword);
                }

                // update success, redirect back to list
                return RedirectToAction("UserMaintenance");
            }
            catch (Exception ex)
            {
                this.LoggingService.Log(String.Format("Exception: {0}", ex.ToString()), "UserEdit", "Admin", 1);

                this.ModelState.AddModelError("", "There was an issue while trying to update the user.");

                return View(vm);
            }
        }

        private List<SelectListItem> GetAllRolesSelectList()
        {
            var rd = this.ReferenceDictionaryProvider.GetReferenceDictionary();

            return
                rd.ApplicationRoles
                .Select(x => new SelectListItem()
                {
                    Value = x,
                    Text = x
                })
                .ToList();
        }

        private List<SelectListItem> GetAllRolesSelectListWithAny()
        {
            var roles = this.GetAllRolesSelectList();

            roles.Insert(0, new SelectListItem() { Value = "", Text = "Any" });

            return roles;
        }

        private List<SelectListItem> GetAllRolesSelectListWithBlank()
        {
            var roles = this.GetAllRolesSelectList();

            roles.Insert(0, new SelectListItem() { Value = "", Text = "" });

            return roles;
        }
    }
}