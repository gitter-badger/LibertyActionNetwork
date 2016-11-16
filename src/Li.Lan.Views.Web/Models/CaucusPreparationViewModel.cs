using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Models
{
    public class CaucusPreparationViewModel
    {
        public int CaucusPreparationId { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public byte ActiveStatus { get; set; }
        
        public int VoterId { get; set; }
        public short ElectionId { get; set; }

        public byte CallDispositionId { get; set; }

        public bool? IsAttending { get; set; }
        public bool? IsDelegate { get; set; }
        public bool? IsCentralCommittee { get; set; }
        public bool? IsVolunteer { get; set; }

        public string Note { get; set; }

        public List<SelectListItem> AllCallDispositionIds { get; set; }

        public VoterEditViewModel VoterEditViewModel { get; set; }

        public List<SelectListItem> All2016IowaCaucusPresidentCandidates { get; set; }

        public List<VoterCandidatePreferenceViewModel> VoterCandidatePreferences2016IowaCaucusPresident { get; set; }

        public int IowaCaucus2016CandidateId { get; set; }

        public List<SelectListItem> All2014IowaPrimarySenateCandidates { get; set; }

        public List<VoterCandidatePreferenceViewModel> VoterCandidatePreferences2014IowaSenate { get; set; }

        public int IowaPrimary2014CandidateId { get; set; }
    }
}