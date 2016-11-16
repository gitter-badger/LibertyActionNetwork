using System;

namespace Li.Lan.Models
{
    public class VoterView
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
        public int? PrecinctId { get; set; }
        public string FacebookId { get; set; }
        public string TwitterId { get; set; }
        public string Email { get; set; }
        public string PrecinctCode { get; set; }
        public string PrecinctDescription { get; set; }
    }
}