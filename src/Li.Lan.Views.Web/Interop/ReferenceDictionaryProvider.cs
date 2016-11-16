using Li.Lan.Data;
using Li.Lan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Li.Lan.Views.Web.Interop
{
    public class ReferenceDictionaryProvider : IReferenceDictionaryProvider
    {
        private const string ApplicationKeyReferenceDictionary = "ApplicationKeyReferenceDictionary";

        private static object LockObject = new object();

        public ReferenceDictionaryProvider(
            IDimensionRepository dimensionRepository,
            HttpContext httpContext)
        {
            this.DimensionRepository = dimensionRepository;

            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            this.HttpContext = httpContext;
        }

        private IDimensionRepository DimensionRepository { get; set; }

        private HttpContext HttpContext { get; set; }

        public ReferenceDictionary GetReferenceDictionary()
        {
            return this.CreateOrRetrieveReferenceDictionary();
        }

        private ReferenceDictionary CreateOrRetrieveReferenceDictionary()
        {
            var referenceDictionary = (ReferenceDictionary)this.HttpContext.Application[ApplicationKeyReferenceDictionary];

            if (referenceDictionary == null)
            {
                lock (LockObject)
                {
                    referenceDictionary = (ReferenceDictionary)this.HttpContext.Application[ApplicationKeyReferenceDictionary];

                    if (referenceDictionary == null)
                    {
                        referenceDictionary = new ReferenceDictionary();

                        referenceDictionary.ApplicationRoles = new string[]
                        {
                            ReferenceDictionary.RoleAdmin
                        };

                        referenceDictionary.Precincts =
                            this.DimensionRepository.SelectPrecincts()
                            .OrderBy(x => x.Description)
                            .ThenBy(x => x.Code)
                            .ToList();

                        referenceDictionary.Cities =
                            this.DimensionRepository.SelectCities()
                            .OrderBy(x => x)
                            .ToList();

                        referenceDictionary.IssueTags =
                            this.DimensionRepository.SelectIssueTags()
                            .OrderBy(x => x.Description)
                            .ToList();

                        referenceDictionary.PhoneNumberTypes = new List<PhoneNumberType>()
                        {
                            ReferenceDictionary.PhoneNumberTypeUnknown,
                            ReferenceDictionary.PhoneNumberTypeMobile,
                            ReferenceDictionary.PhoneNumberTypeHome,
                            ReferenceDictionary.PhoneNumberTypeBusiness,
                        };

                        referenceDictionary.CandidateSupportLevels = new List<SupportLevel>()
                        {
                            new SupportLevel() { SupportLevelId = ReferenceDictionary.CandidateSupportLevelUnknown, Name = "Unknown" },
                            new SupportLevel() { SupportLevelId = ReferenceDictionary.CandidateSupportLevelStrong, Name = "Strong" },
                            new SupportLevel() { SupportLevelId = ReferenceDictionary.CandidateSupportLevelAverage, Name = "Average" },
                            new SupportLevel() { SupportLevelId = ReferenceDictionary.CandidateSupportLevelWeak, Name = "Weak" },
                        };

                        // Temp: Only use subset
                        ////referenceDictionary.IowaCaucus2016PresidentCandidates =
                        ////    this.DimensionRepository
                        ////        .SelectCandidates(
                        ////            ReferenceDictionary.ElectionIdIowaCaucus2016,
                        ////            ReferenceDictionary.PositionIdPresidnet)
                        ////        .OrderBy(x => x.Name)
                        ////        .ToList();

                        referenceDictionary.IowaCaucus2016PresidentCandidates =
                            new List<Candidate>()
                            {
                                new Candidate() { CandidateId = 32, Name = "Undecided"},
                                new Candidate() { CandidateId = 14, Name = "Chris Christie"},
                                new Candidate() { CandidateId = 20, Name = "Marco Rubio"},
                                new Candidate() { CandidateId = 23, Name = "Paul Ryan"},
                                new Candidate() { CandidateId = 12, Name = "Rand Paul"},
                                new Candidate() { CandidateId = 24, Name = "Ted Cruz"},
                            };

                        referenceDictionary.IowaPrimary2014SenateCandidates =
                            this.DimensionRepository
                                .SelectCandidates(
                                    ReferenceDictionary.ElectionIdIowaPrimaryElection2014,
                                    ReferenceDictionary.PositionIdIowaSenate01)
                                .OrderBy(x => x.Name)
                                .ToList();

                        this.HttpContext.Application[ApplicationKeyReferenceDictionary] = referenceDictionary;
                    }
                }
            }

            return referenceDictionary;
        }
    }
}