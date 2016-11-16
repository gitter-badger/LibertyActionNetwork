using System;

namespace Li.Lan.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public DateTime InsertedOn { get; set; }
        public short ElectionId { get; set; }
        public int PositionId { get; set; }
        public string Name { get; set; }
    }
}