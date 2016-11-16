using Li.Lan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Li.Lan.Views.Web.Models
{
    public class Mapper
    {
        public static VoterViewViewModel Map(VoterView m)
        {
            var vm = new VoterViewViewModel();

            vm.ActiveStatus = m.ActiveStatus;
            vm.AddressLine1 = m.AddressLine1;
            vm.AddressLine2 = m.AddressLine2;
            vm.City = m.City;
            vm.FacebookId = m.FacebookId;
            vm.FirstName = m.FirstName;
            vm.LastName = m.LastName;
            vm.PhoneNumber = m.PhoneNumber;
            vm.PrecinctCode = m.PrecinctCode;
            vm.PrecinctDescription = m.PrecinctDescription;
            vm.PrecinctId = m.PrecinctId;
            vm.StateCode = m.StateCode;
            vm.StateVoterId = m.StateVoterId;
            vm.TwitterId = m.TwitterId;
            vm.VoterGuid = m.VoterGuid;
            vm.VoterId = m.VoterId;
            vm.ZipCode = m.ZipCode;

            return vm;
        }

        public static VoterIssueTagViewModel Map(VoterIssueTagView m)
        {
            var vm = new VoterIssueTagViewModel();

            vm.VoterIssueTagId = m.VoterIssueTagId;
            vm.VoterId = m.VoterId;
            vm.IssueTagId = m.IssueTagId;
            vm.IssueTagDescription = m.IssueTagDescription;
            vm.Priority = m.Priority;

            return vm;
        }

        public static VoterElectionViewModel Map(VoterElectionView m)
        {
            var vm = new VoterElectionViewModel();

            vm.VoterElectionId = m.VoterElectionId;
            vm.InsertedBy = m.InsertedBy;
            vm.InsertedOn = m.InsertedOn;
            vm.UpdatedBy = m.UpdatedBy;
            vm.UpdatedOn = m.UpdatedOn;
            vm.ActiveStatus = m.ActiveStatus;
            vm.VoterId = m.VoterId;
            vm.ElectionId = m.ElectionId;
            vm.CandidateId = m.CandidateId;
            vm.ElectionDate = m.ElectionDate;
            vm.ElectionName = m.ElectionName;
            vm.ElectionTypeId = m.ElectionTypeId;

            return vm;
        }

        public static VoterCandidatePreferenceViewModel Map(VoterCandidatePreferenceView m)
        {
            var vm = new VoterCandidatePreferenceViewModel();

            vm.ActiveStatus = m.ActiveStatus;
            vm.CandidateId = m.CandidateId;
            vm.CandidateName = m.CandidateName;
            vm.FromMigrationFlag = m.FromMigrationFlag;
            vm.InsertedBy = m.InsertedBy;
            vm.InsertedOn = m.InsertedOn;
            vm.Priority = m.Priority;
            vm.SupportLevel = m.SupportLevel;
            vm.SupportLevelDescription = m.SupportLevelDescription;
            vm.UpdatedBy = m.UpdatedBy;
            vm.UpdatedOn = m.UpdatedOn;
            vm.VoterCandidatePreferenceId = m.VoterCandidatePreferenceId;
            vm.VoterId = m.VoterId;

            return vm;
        }

        public static VoterEditViewModel MapEdit(Voter m)
        {
            var vm = new VoterEditViewModel();

            vm.VoterId = m.VoterId;
            vm.VoterGuid = m.VoterGuid;
            vm.ActiveStatus = m.ActiveStatus;

            vm.StateVoterId = m.StateVoterId;

            vm.FirstName = m.FirstName;
            vm.NickName = m.NickName;
            vm.LastName = m.LastName;

            vm.AddressLine1 = m.AddressLine1;
            vm.AddressLine2 = m.AddressLine2;
            vm.City = m.City;
            vm.StateCode = m.StateCode;
            vm.ZipCode = m.ZipCode;

            vm.NewAddressLine1 = m.NewAddressLine1;
            vm.NewAddressLine2 = m.NewAddressLine2;
            vm.NewCity = m.NewCity;
            vm.NewStateCode = m.NewStateCode;
            vm.NewZipCode = m.NewZipCode;

            vm.PhoneNumber = m.PhoneNumber;
            vm.PhoneNumberType = m.PhoneNumberType;

            vm.PhoneNumber2 = m.PhoneNumber2;
            vm.PhoneNumber2Type = m.PhoneNumber2Type;

            vm.PrecinctId = m.PrecinctId;

            vm.TwitterId = m.TwitterId;
            vm.FacebookId = m.FacebookId;

            vm.Email = m.Email;

            return vm;
        }

        public static UserViewModel Map(Li.Lan.Models.UserProfile m)
        {
            var vm = new UserViewModel();

            vm.UserId = m.UserId;
            vm.UserName = m.UserName;

            var role = m.Roles.FirstOrDefault();

            if (role != null)
            {
                vm.RoleId = role.RoleId;
                vm.RoleName = role.RoleName;
            }
            else
            {
                vm.RoleId = 0;
                vm.RoleName = "";
            }

            var precinctTags =
                m.PrecinctTags
                .Select(x => new PrecinctTagViewModel()
                {
                    PrecinctTagId = x.PrecinctTagId,
                    Description = x.Description
                })
                .ToList();

            vm.PrecinctTags = precinctTags;

            return vm;
        }
    }
}