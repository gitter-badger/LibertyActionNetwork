using System;

namespace Li.Lan.Models
{
    public class VoterIssueTag
    {
        public int VoterIssueTagId { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public byte ActiveStatus { get; set; }
        public int VoterId { get; set; }
        public short IssueTagId { get; set; }
        public byte Priority { get; set; }
        public byte? FromMigrationFlag { get; set; }
    }
}