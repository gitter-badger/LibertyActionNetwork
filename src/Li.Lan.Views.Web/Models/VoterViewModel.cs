using System;
using System.ComponentModel.DataAnnotations;

namespace Li.Lan.Views.Web.Models
{
    public class VoterViewModel
    {
        public VoterViewModel()
        {
            // no-op
        }

        public int VoterId { get; set; }
        public Guid VoterGuid { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public byte ActiveStatus { get; set; }
        
        public string StateVoterId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(250)]
        public string AddressLine1 { get; set; }

        [StringLength(250)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string StateCode { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5)]
        [RegularExpression(@"^\d{5}(\-\d{4})?$")]
        public string ZipCode { get; set; }

        [StringLength(12, MinimumLength = 10)]
        [RegularExpression(@"^\d{3}(\-)?\d{3}(\-)?\d{4}$")]
        public string PhoneNumber { get; set; }

        public int? PrecinctId { get; set; }
        
        public string FacebookId { get; set; }

        public string TwitterId { get; set; }
    }
}