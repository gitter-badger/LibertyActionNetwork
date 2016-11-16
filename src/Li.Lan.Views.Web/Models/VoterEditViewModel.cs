using Li.Lan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Models
{
    public class VoterEditViewModel
    {
        public int VoterId { get; set; }
        public Guid VoterGuid { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public byte ActiveStatus { get; set; }

        public string StateVoterId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
        public byte? PhoneNumberType { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        [StringLength(250)]
        public string NewAddressLine1 { get; set; }

        [StringLength(250)]
        public string NewAddressLine2 { get; set; }

        [StringLength(100)]
        public string NewCity { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string NewStateCode { get; set; }

        [StringLength(10, MinimumLength = 5)]
        [RegularExpression(@"^\d{5}(\-\d{4})?$")]
        public string NewZipCode { get; set; }

        [StringLength(12, MinimumLength = 10)]
        [RegularExpression(@"^\d{3}(\-)?\d{3}(\-)?\d{4}$")]
        public string PhoneNumber2 { get; set; }

        public byte? PhoneNumber2Type { get; set; }

        public int? PrecinctId { get; set; }

        public string FacebookId { get; set; }

        public string TwitterId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the selected VoterIssueTags
        /// </summary>
        public List<VoterIssueTagViewModel> VoterIssueTagViewModels { get; set; }

        /// <summary>
        /// Gets or sets all possible IssueTags to choose from
        /// </summary>
        public List<SelectListItem> AllIssueTags { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public List<SelectListItem> PhoneNumberTypes { get; set; }

        public List<SelectListItem> AllIowaCaucus2016Candidates { get; set; }

        public List<VoterCandidatePreferenceViewModel> VoterCandidatePreferencesIowaCaucus2016 { get; set; }
    }
}