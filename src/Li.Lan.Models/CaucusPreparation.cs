using System;

namespace Li.Lan.Models
{
    public class CaucusPreparation
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
        public byte? IsAttending { get; set; }
        public byte? IsDelegate { get; set; }
        public byte? IsCentralCommittee { get; set; }
        public byte? IsVolunteer { get; set; }
        public string Note { get; set; }
    }
}