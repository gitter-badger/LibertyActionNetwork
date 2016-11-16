using Li.Lan.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Li.Lan.Views.Web.Models
{
    public class VoterSearchViewModel
    {
        public VoterSearchViewModel()
        {
            this.VoterSearchOptions = new VoterSearchOptions();
            this.Results = new List<VoterView>();
        }

        public VoterSearchOptions VoterSearchOptions { get; set; }

        public IEnumerable<SelectListItem> Precincts { get; set; }

        public IEnumerable<SelectListItem> IssueTags { get; set; }

        public IEnumerable<VoterView> Results { get; set; }
    }
}