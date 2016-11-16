using System;
using System.Collections.Generic;

namespace Li.Lan.Models
{
    public class ReferenceDictionary
    {
        public const string ApplicationVersion = "01.20130805.01";

        public const string RoleAdmin = "Admin";

        public const byte ByteTrue = 1;
        public const byte ByteFalse = 0;

        public const byte CandidateSupportLevelUnknown = 0;
        public const byte CandidateSupportLevelStrong = 1;
        public const byte CandidateSupportLevelAverage = 2;
        public const byte CandidateSupportLevelWeak = 3;

        public const byte ActiveStatusActive = 1;
        public static readonly List<byte> ActiveStatusActiveArray = new List<byte> { 1 };

        public static readonly PhoneNumberType PhoneNumberTypeUnknown = new PhoneNumberType() { PhoneNumberTypeId = 1, Description = "Unknown" };
        public static readonly PhoneNumberType PhoneNumberTypeMobile = new PhoneNumberType() { PhoneNumberTypeId = 2, Description = "Mobile" };
        public static readonly PhoneNumberType PhoneNumberTypeHome = new PhoneNumberType() { PhoneNumberTypeId = 3, Description = "Home" };
        public static readonly PhoneNumberType PhoneNumberTypeBusiness = new PhoneNumberType() { PhoneNumberTypeId = 4, Description = "Business" };

        public const short ElectionIdIowaCaucus2016 = 44;
        public const short ElectionIdIowaCaucus2012 = 1;
        public const short ElectionIdIowaPrimaryElection2014 = 42;
        public const short ElectionIdIowaCaucus2014 = 45;

        public const int PositionIdIowaSenate01 = 3;
        public const int PositionIdPresidnet = 2;

        public IEnumerable<string> ApplicationRoles { get; set; }

        public IEnumerable<Precinct> Precincts { get; set; }

        public IEnumerable<String> Cities { get; set; }

        public IEnumerable<IssueTag> IssueTags { get; set; }

        public IEnumerable<PhoneNumberType> PhoneNumberTypes { get; set; }

        public IEnumerable<Candidate> IowaCaucus2016PresidentCandidates { get; set; }

        public IEnumerable<Candidate> IowaPrimary2014SenateCandidates { get; set; }

        public IEnumerable<SupportLevel> CandidateSupportLevels { get; set; }
    }
}