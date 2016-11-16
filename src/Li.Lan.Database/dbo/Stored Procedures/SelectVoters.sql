CREATE PROCEDURE [dbo].[SelectVoters]
(
	@ResultLimit int = 100,
	@LastName nvarchar(50) = NULL,
	@StreetNameLike nvarchar(250) = NULL,
	@PrecinctIds dbo.IntTableParameterType readonly,
	@PrecinctTags dbo.IntTableParameterType readonly,
	@IssueTags dbo.SmallintTableParameterType readonly,
	@CandidatePreferenceIds dbo.IntTableParameterType readonly
)
AS
	declare @PrecinctIdsCount int = (select count(*) from @PrecinctIds)
	declare @PrecinctTagsCount int = (select count(*) from @PrecinctTags)
	declare @IssueTagsCount int = (select count(*) from @IssueTags)
	declare @CandidatePreferenceIdsCount int = (select count(*) from @CandidatePreferenceIds)

	select
		 v.VoterId
		,v.VoterGuid
		,v.InsertedBy
		,v.InsertedOn
		,v.UpdatedBy
		,v.UpdatedOn
		,v.ActiveStatus
		,v.StateVoterId
		,v.FirstName
		,v.LastName
		,v.AddressLine1
		,v.AddressLine2
		,v.City
		,v.StateCode
		,v.ZipCode
		,v.PhoneNumber
		,v.PrecinctId
		,v.FacebookId
		,v.TwitterId
		,p.Code PrecinctCode
		,p.Description PrecinctDescription
		
	from
		dbo.Voter v
		left join dbo.Precinct p on (v.PrecinctId = p.PrecinctId)
		
		join (
			select  top (@ResultLimit)
				v2.VoterId
				
			from
				dbo.Voter v2
				
				left join @PrecinctIds pids on (v2.PrecinctId = pids.Id)
				
				left join dbo.PrecinctPrecinctTag ppt on (v2.PrecinctId = ppt.PrecinctId)
				left join @PrecinctTags pt on (ppt.PrecinctTagId = pt.Id)

				left join dbo.VoterIssueTag vit on (v2.VoterId = vit.VoterId)
				left join @IssueTags it on (vit.IssueTagId = it.Id)

				left join dbo.VoterCandidatePreference vcp on (v2.VoterId = vcp.VoterId)
				left join @CandidatePreferenceIds cp on (vcp.CandidateId = cp.Id)
			
			where
				(@LastName is null or v2.LastName = @LastName)
				and (@StreetNameLike is null or v2.AddressLine1 like @StreetNameLike)

				and (@PrecinctIdsCount = 0 or pids.Id is not null)

				and (@PrecinctTagsCount = 0 or pt.Id is not null)

				and (@IssueTagsCount = 0 or it.Id is not null)

				and (@CandidatePreferenceIdsCount = 0 or cp.Id is not null)
		
			group by
				v2.VoterId
		
		) x on v.VoterId = x.VoterId

	where
		v.ActiveStatus = 1