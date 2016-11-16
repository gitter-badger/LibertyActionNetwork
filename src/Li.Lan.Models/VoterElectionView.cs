﻿using System;

namespace Li.Lan.Models
{
    public class VoterElectionView
    {
        public int VoterElectionId { get; set; }

        public int InsertedBy { get; set; }

        public DateTime InsertedOn { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public byte ActiveStatus { get; set; }

        public int VoterId { get; set; }

        public short ElectionId { get; set; }

        public int? CandidateId { get; set; }

        public byte? FromMigrationFlag { get; set; }

        public DateTime ElectionDate { get; set; }

        public string ElectionName { get; set; }

        public byte ElectionTypeId { get; set; }
    }
}