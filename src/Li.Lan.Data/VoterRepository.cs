using Li.Lan.Common.Data;
using Li.Lan.Models;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Li.Lan.Data
{
    public interface IVoterRepository
    {
        Voter InsertVoter(Voter voter);

        Voter UpdateVoter(Voter voter);

        Voter SelectVoter(int voterId);

        Voter SelectVoter(string stateVoterId);

        VoterView SelectVoterView(int voterId);

        IEnumerable<VoterView> SelectVoters(VoterSearchOptions voterSearchOptions);

        IEnumerable<VoterIssueTagView> SelectVoterIssueTagViews(int voterId);

        IEnumerable<VoterIssueTag> UpdateVoterIssueTagsForVoter(int voterId, IEnumerable<VoterIssueTag> newVoterIssueTags);

        IEnumerable<VoterElectionView> SelectVoterElectionViews(int voterId);

        IEnumerable<VoterCandidatePreferenceView> SelectVoterCandidatePreferenceView(VoterCandidatePreferenceSearchOptions options);

        IEnumerable<VoterCandidatePreference> UpdateVoterCandidatePreference(int voterId, short electionId, IEnumerable<VoterCandidatePreference> newVoterCandidatePreferences);

        CaucusPreparation InsertCaucusPreparation(CaucusPreparation caucusPreparation);

        CaucusPreparation UpdateCaucusPreparation(CaucusPreparation caucusPreparation);

        CaucusPreparation SelectCaucusPreparation(short electionId, int voterId);
    }

    public class VoterRepository : BaseRepository, IVoterRepository
    {
        public VoterRepository(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider)
        {
            // no-op
        }

        public Voter InsertVoter(Voter voter)
        {
            using (var ctx = this.CreateContext())
            {
                ctx.Voters.Add(voter);
                ctx.SaveChanges();
            }

            return voter;
        }

        public Voter UpdateVoter(Voter voter)
        {
            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.Voters
                    where v.VoterId == voter.VoterId
                    select v;

                var db = query.SingleOrDefault();

                if (db != null)
                {
                    db.ActiveStatus = voter.ActiveStatus;

                    db.NickName = voter.NickName;

                    db.PhoneNumber2 = voter.PhoneNumber2;
                    db.PhoneNumber2Type = voter.PhoneNumber2Type;

                    db.NewAddressLine1 = voter.NewAddressLine1;
                    db.NewAddressLine2 = voter.NewAddressLine2;
                    db.NewCity = voter.NewCity;
                    db.NewStateCode = voter.NewStateCode;
                    db.NewZipCode = voter.NewZipCode;

                    db.TwitterId = voter.TwitterId;
                    db.FacebookId = voter.FacebookId;

                    db.Email = voter.Email;

                    db.UpdatedBy = voter.UpdatedBy;
                    db.UpdatedOn = voter.UpdatedOn;

                    ctx.SaveChanges();
                }
            }

            return voter;
        }

        public Voter SelectVoter(int voterId)
        {
            Voter result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.Voters
                    where v.VoterId == voterId
                    select v;

                result = query.SingleOrDefault();
            }

            return result;
        }

        public Voter SelectVoter(string stateVoterId)
        {
            Voter result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.Voters
                    where v.StateVoterId == stateVoterId
                    select v;

                result = query.SingleOrDefault();
            }

            return result;
        }

        public VoterView SelectVoterView(int voterId)
        {
            VoterView result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.Voters
                    join p in ctx.Precincts on v.PrecinctId equals p.PrecinctId into vp
                    from p in vp.DefaultIfEmpty()
                    where
                        ReferenceDictionary.ActiveStatusActiveArray.Contains(v.ActiveStatus)
                        && v.VoterId == voterId
                    select new { v, p };

                result =
                    query
                    .Select(x => new VoterView()
                    {
                        ActiveStatus = x.v.ActiveStatus,
                        AddressLine1 = x.v.AddressLine1,
                        AddressLine2 = x.v.AddressLine2,
                        City = x.v.City,
                        Email = x.v.Email,
                        FacebookId = x.v.FacebookId,
                        FirstName = x.v.FirstName,
                        InsertedBy = x.v.InsertedBy,
                        InsertedOn = x.v.InsertedOn,
                        LastName = x.v.LastName,
                        PhoneNumber = x.v.PhoneNumber,
                        PrecinctCode = x.p == null ? "" : x.p.Code,
                        PrecinctDescription = x.p == null ? "" : x.p.Description,
                        PrecinctId = x.v.PrecinctId,
                        StateCode = x.v.StateCode,
                        StateVoterId = x.v.StateVoterId,
                        TwitterId = x.v.TwitterId,
                        UpdatedBy = x.v.UpdatedBy,
                        UpdatedOn = x.v.UpdatedOn,
                        VoterGuid = x.v.VoterGuid,
                        VoterId = x.v.VoterId,
                        ZipCode = x.v.ZipCode,
                    })
                    .SingleOrDefault();
            }

            return result;
        }

        public IEnumerable<VoterView> SelectVoters(VoterSearchOptions voterSearchOptions)
        {
            List<VoterView> result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.Voters
                    join p in ctx.Precincts on v.PrecinctId equals p.PrecinctId into vp
                    from p in vp.DefaultIfEmpty()
                    where v.ActiveStatus == ReferenceDictionary.ActiveStatusActive
                    select new { v, p };

                if (!String.IsNullOrWhiteSpace(voterSearchOptions.LastName))
                {
                    var l = voterSearchOptions.LastName.ToUpper();
                    query = query.Where(x => x.v.LastName == l);
                }

                if (!String.IsNullOrWhiteSpace(voterSearchOptions.StreetNameLike))
                {
                    var s = voterSearchOptions.StreetNameLike.ToUpper();
                    query = query.Where(x => x.v.AddressLine1.Contains(s));
                }

                if (voterSearchOptions.PrecinctIds != null
                    && voterSearchOptions.PrecinctIds.Count() > 0)
                {
                    if (voterSearchOptions.PrecinctIds.Count() == 1)
                    {
                        var pid = voterSearchOptions.PrecinctIds.First();
                        query = query.Where(x => x.v.PrecinctId == pid);
                    }
                    else
                    {
                        query = query.Where(x => x.v.PrecinctId.HasValue && voterSearchOptions.PrecinctIds.Contains(x.v.PrecinctId.Value));
                    }
                }

                if (voterSearchOptions.IssueTags.Any())
                {
                    var vit =
                        from v in ctx.VoterIssueTags
                        where
                            ReferenceDictionary.ActiveStatusActiveArray.Contains(v.ActiveStatus)
                            && voterSearchOptions.IssueTags.Contains(v.IssueTagId)
                        select v.VoterId;

                    query = query.Where(x => vit.Contains(x.v.VoterId));
                }

                voterSearchOptions.TotalResultCount = query.Count();

                result =
                    query
                    .Take(voterSearchOptions.ResultLimit)
                    .Select(x => new VoterView()
                    {
                        ActiveStatus = x.v.ActiveStatus,
                        AddressLine1 = x.v.AddressLine1,
                        AddressLine2 = x.v.AddressLine2,
                        City = x.v.City,
                        Email = x.v.Email,
                        FacebookId = x.v.FacebookId,
                        FirstName = x.v.FirstName,
                        InsertedBy = x.v.InsertedBy,
                        InsertedOn = x.v.InsertedOn,
                        LastName = x.v.LastName,
                        PhoneNumber = x.v.PhoneNumber,
                        PrecinctCode = x.p == null ? "" : x.p.Code,
                        PrecinctDescription = x.p == null ? "" : x.p.Description,
                        PrecinctId = x.v.PrecinctId,
                        StateCode = x.v.StateCode,
                        StateVoterId = x.v.StateVoterId,
                        TwitterId = x.v.TwitterId,
                        UpdatedBy = x.v.UpdatedBy,
                        UpdatedOn = x.v.UpdatedOn,
                        VoterGuid = x.v.VoterGuid,
                        VoterId = x.v.VoterId,
                        ZipCode = x.v.ZipCode,
                    })
                    .ToList();
            }

            return result;
        }

        private IEnumerable<VoterView> SelectVotersStoredProcedure(VoterSearchOptions voterSearchOptions)
        {
            var result = new List<VoterView>();

            using (var sqlConnection = this.CreateSqlConnection())
            {
                var cmd = this.CreateSqlCommandStoredProcedure("dbo.SelectVoters", sqlConnection);

                cmd.Parameters.Add("@ResultLimit", SqlDbType.Int).Value = voterSearchOptions.ResultLimit;

                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = voterSearchOptions.LastName;
                cmd.Parameters.Add("@StreetNameLike", SqlDbType.NVarChar, 250).Value = this.EnsureWildcard(voterSearchOptions.StreetNameLike);

                cmd.Parameters.AddWithValue("@PrecinctIds", CollectionToDataTableConverter.CreateDataTable(voterSearchOptions.PrecinctIds));
                cmd.Parameters.AddWithValue("@PrecinctTags", CollectionToDataTableConverter.CreateDataTable(voterSearchOptions.PrecinctTags));
                cmd.Parameters.AddWithValue("@IssueTags", CollectionToDataTableConverter.CreateDataTable(voterSearchOptions.IssueTags));
                cmd.Parameters.AddWithValue("@CandidatePreferenceIds", CollectionToDataTableConverter.CreateDataTable(voterSearchOptions.CandidateTags));

                sqlConnection.Open();

                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var v = new VoterView();

                    v.VoterId = rdr.GetInt32(0);
                    v.VoterGuid = rdr.GetGuid(1);
                    v.InsertedBy = rdr.GetInt32(2);
                    v.InsertedOn = rdr.GetDateTime(3);
                    v.UpdatedBy = rdr.GetInt32(4);
                    v.UpdatedOn = rdr.GetDateTime(5);
                    v.ActiveStatus = rdr.GetByte(6);
                    v.StateVoterId = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
                    v.FirstName = rdr.GetString(8);
                    v.LastName = rdr.GetString(9);
                    v.AddressLine1 = rdr.GetString(10);
                    v.AddressLine2 = rdr.IsDBNull(11) ? "" : rdr.GetString(11);
                    v.City = rdr.GetString(12);
                    v.StateCode = rdr.GetString(13);
                    v.ZipCode = rdr.IsDBNull(14) ? "" : rdr.GetString(14);
                    v.PhoneNumber = rdr.IsDBNull(15) ? "" : rdr.GetString(15);
                    v.PrecinctId = rdr.IsDBNull(16) ? (int?)null : rdr.GetInt32(16);
                    v.FacebookId = rdr.IsDBNull(17) ? "" : rdr.GetString(17);
                    v.TwitterId = rdr.IsDBNull(18) ? "" : rdr.GetString(18);
                    v.PrecinctCode = rdr.IsDBNull(19) ? "" : rdr.GetString(19);
                    v.PrecinctDescription = rdr.IsDBNull(20) ? "" : rdr.GetString(20);

                    result.Add(v);
                }
            }

            return result;
        }

        public IEnumerable<VoterIssueTagView> SelectVoterIssueTagViews(int voterId)
        {
            IEnumerable<VoterIssueTagView> result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.VoterIssueTags
                    join i in ctx.IssueTags on v.IssueTagId equals i.IssueTagId
                    where
                        ReferenceDictionary.ActiveStatusActiveArray.Contains(v.ActiveStatus)
                        && v.VoterId == voterId
                    select new VoterIssueTagView()
                    {
                        VoterIssueTagId = v.VoterIssueTagId,
                        VoterId = v.VoterId,
                        IssueTagId = v.IssueTagId,
                        IssueTagDescription = i.Description,
                        Priority = v.Priority
                    };

                result = query.ToList();
            }

            return result;
        }

        public IEnumerable<VoterIssueTag> UpdateVoterIssueTagsForVoter(int voterId, IEnumerable<VoterIssueTag> newVoterIssueTags)
        {
            if (!newVoterIssueTags.Any())
                return newVoterIssueTags;

            using (var ctx = this.CreateContext())
            {
                // get existing tags and deactivate them
                var query =
                    from vit in ctx.VoterIssueTags
                    where
                        ReferenceDictionary.ActiveStatusActiveArray.Contains(vit.ActiveStatus)
                        && vit.VoterId == voterId
                    select vit;

                var existingTags = query.ToList();

                var userId = newVoterIssueTags.First().UpdatedBy;
                var utc = newVoterIssueTags.First().UpdatedOn;

                foreach (var tag in existingTags)
                {
                    tag.ActiveStatus = 0;
                    tag.UpdatedBy = userId;
                    tag.UpdatedOn = utc;
                }

                // insert new tags
                foreach (var tag in newVoterIssueTags)
                {
                    ctx.VoterIssueTags.Add(tag);
                }

                ctx.SaveChanges();
            }

            return newVoterIssueTags;
        }

        public IEnumerable<VoterElectionView> SelectVoterElectionViews(int voterId)
        {
            IEnumerable<VoterElectionView> result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from v in ctx.VoterElections
                    join e in ctx.Elections on v.ElectionId equals e.ElectionId
                    where
                        v.VoterId == voterId
                        && ReferenceDictionary.ActiveStatusActiveArray.Contains(v.ActiveStatus)
                    select new VoterElectionView()
                    {
                        VoterElectionId = v.VoterElectionId,
                        InsertedBy = v.InsertedBy,
                        InsertedOn = v.InsertedOn,
                        UpdatedBy = v.UpdatedBy,
                        UpdatedOn = v.UpdatedOn,
                        ActiveStatus = v.ActiveStatus,
                        VoterId = v.VoterId,
                        ElectionId = v.ElectionId,
                        CandidateId = v.CandidateId,
                        ElectionDate = e.ElectionDate,
                        ElectionName = e.Name,
                        ElectionTypeId = e.ElectionTypeId,
                    };

                result = query.ToList();
            }

            return result;
        }

        public IEnumerable<VoterCandidatePreferenceView> SelectVoterCandidatePreferenceView(VoterCandidatePreferenceSearchOptions options)
        {
            IEnumerable<VoterCandidatePreferenceView> result;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from p in ctx.VoterCandidatePreferences
                    join c in ctx.Candidates on p.CandidateId equals c.CandidateId
                    where ReferenceDictionary.ActiveStatusActiveArray.Contains(p.ActiveStatus)
                    select new { p, c };

                if (options.VoterId.HasValue)
                {
                    query = query.Where(x => x.p.VoterId == options.VoterId.Value);
                }

                if (options.ElectionId.HasValue)
                {
                    query = query.Where(x => x.c.ElectionId == options.ElectionId.Value);
                }

                if (options.PositionId.HasValue)
                {
                    query = query.Where(x => x.c.PositionId == options.PositionId.Value);
                }

                result =
                    query
                    .Select(x => new VoterCandidatePreferenceView()
                    {
                        VoterCandidatePreferenceId = x.p.VoterCandidatePreferenceId,
                        InsertedBy = x.p.InsertedBy,
                        InsertedOn = x.p.InsertedOn,
                        UpdatedBy = x.p.UpdatedBy,
                        UpdatedOn = x.p.UpdatedOn,
                        ActiveStatus = x.p.ActiveStatus,
                        VoterId = x.p.VoterId,
                        CandidateId = x.p.CandidateId,
                        Priority = x.p.Priority,
                        SupportLevel = x.p.SupportLevel,
                        FromMigrationFlag = x.p.FromMigrationFlag,
                        CandidateName = x.c.Name,
                        // exception: can not be compiled...
                        //SupportLevelDescription = this.GetSupportLevelDescription(x.p.SupportLevel),
                    })
                    .ToList();

                foreach (var r in result)
                {
                    r.SupportLevelDescription = this.GetSupportLevelDescription(r.SupportLevel);
                }
            }

            return result;
        }

        public IEnumerable<VoterCandidatePreference> UpdateVoterCandidatePreference(int voterId, short electionId, IEnumerable<VoterCandidatePreference> newVoterCandidatePreferences)
        {
            if (!newVoterCandidatePreferences.Any())
                return newVoterCandidatePreferences;

            using (var ctx = this.CreateContext())
            {
                // get existing perfs and deactivate them
                var query =
                    from p in ctx.VoterCandidatePreferences
                    join c in ctx.Candidates on p.CandidateId equals c.CandidateId
                    where
                        ReferenceDictionary.ActiveStatusActiveArray.Contains(p.ActiveStatus)
                        && p.VoterId == voterId
                        && c.ElectionId == electionId
                    select p;

                var existingPerfs = query.ToList();

                var userId = newVoterCandidatePreferences.First().UpdatedBy;
                var utc = newVoterCandidatePreferences.First().UpdatedOn;

                foreach (var tag in existingPerfs)
                {
                    tag.ActiveStatus = 0;
                    tag.UpdatedBy = userId;
                    tag.UpdatedOn = utc;
                }

                // insert new tags
                foreach (var perf in newVoterCandidatePreferences)
                {
                    ctx.VoterCandidatePreferences.Add(perf);
                }

                ctx.SaveChanges();
            }

            return newVoterCandidatePreferences;
        }

        public CaucusPreparation InsertCaucusPreparation(CaucusPreparation caucusPreparation)
        {
            using (var ctx = this.CreateContext())
            {
                ctx.CaucusPreparations.Add(caucusPreparation);
                ctx.SaveChanges();
            }

            return caucusPreparation;
        }

        public CaucusPreparation UpdateCaucusPreparation(CaucusPreparation caucusPreparation)
        {
            using (var ctx = this.CreateContext())
            {
                var query =
                    from cp in ctx.CaucusPreparations
                    where cp.CaucusPreparationId == caucusPreparation.CaucusPreparationId
                    select cp;

                var db = query.SingleOrDefault();

                if (db != null)
                {
                    db.ActiveStatus = caucusPreparation.ActiveStatus;

                    db.CallDispositionId = caucusPreparation.CallDispositionId;
                    db.IsAttending = caucusPreparation.IsAttending;
                    db.IsDelegate = caucusPreparation.IsDelegate;
                    db.IsCentralCommittee = caucusPreparation.IsCentralCommittee;
                    db.IsVolunteer = caucusPreparation.IsVolunteer;

                    db.Note = caucusPreparation.Note;

                    db.UpdatedBy = caucusPreparation.UpdatedBy;
                    db.UpdatedOn = caucusPreparation.UpdatedOn;

                    ctx.SaveChanges();
                }
            }

            return caucusPreparation;
        }

        public CaucusPreparation SelectCaucusPreparation(short electionId, int voterId)
        {
            CaucusPreparation result = null;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from cp in ctx.CaucusPreparations
                    where
                        cp.VoterId == voterId
                        && cp.ElectionId == electionId
                    select cp;

                result = query.SingleOrDefault();
            }

            return result;
        }

        private string GetSupportLevelDescription(byte supportLevel)
        {
            switch (supportLevel)
            {
                case ReferenceDictionary.CandidateSupportLevelUnknown: return "Unknown";
                case ReferenceDictionary.CandidateSupportLevelStrong: return "Strong";
                case ReferenceDictionary.CandidateSupportLevelAverage: return "Average";
                case ReferenceDictionary.CandidateSupportLevelWeak: return "Weak";
            }

            return "Unknown";
        }
    }
}