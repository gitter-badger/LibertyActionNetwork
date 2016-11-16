using System;

namespace Li.Lan.Models
{
    public class VoterCandidatePreferenceView
    {
        public int VoterCandidatePreferenceId { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public byte ActiveStatus { get; set; }
        public int VoterId { get; set; }
        public int CandidateId { get; set; }
        public byte Priority { get; set; }
        public byte SupportLevel { get; set; }
        public byte? FromMigrationFlag { get; set; }
        
        public string CandidateName { get; set; }

        public string SupportLevelDescription { get; set; }
    }
}