using System.Collections.Generic;

namespace Li.Lan.Models
{
    public class VoterSearchOptions
    {
        public VoterSearchOptions()
        {
            // default result limit to 100
            this.ResultLimit = 100;
            this.PrecinctIds = new List<int>();
            this.PrecinctTags = new List<int>();
            this.IssueTags = new List<short>();
            this.CandidateTags = new List<int>();
        }

        public int ResultLimit { get; set; }

        public int TotalResultCount { get; set; }

        public string LastName { get; set; }
        
        public string StreetNameLike { get; set; }

        public IEnumerable<int> PrecinctIds { get; set; }

        public IEnumerable<int> PrecinctTags { get; set; }

        public IEnumerable<short> IssueTags { get; set; }

        public IEnumerable<int> CandidateTags { get; set; }
    }
}