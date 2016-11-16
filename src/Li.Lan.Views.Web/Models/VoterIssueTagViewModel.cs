using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Li.Lan.Views.Web.Models
{
    public class VoterIssueTagViewModel
    {
        public int VoterIssueTagId { get; set; }

        public int VoterId { get; set; }

        public short IssueTagId { get; set; }

        public string IssueTagDescription { get; set; }

        public byte Priority { get; set; }
    }
}