using System;

namespace Li.Lan.Models
{
    public class Election
    {
        public short ElectionId { get; set; }
        public DateTime ElectionDate { get; set; }
        public string Name { get; set; }
        public byte ElectionTypeId { get; set; }
    }
}