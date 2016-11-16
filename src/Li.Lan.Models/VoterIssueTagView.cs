namespace Li.Lan.Models
{
    public class VoterIssueTagView
    {
        public int VoterIssueTagId { get; set; }

        public int VoterId { get; set; }

        public short IssueTagId { get; set; }

        public string IssueTagDescription { get; set; }

        public byte Priority { get; set; }
    }
}