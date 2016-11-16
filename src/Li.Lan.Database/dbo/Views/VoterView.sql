CREATE VIEW [dbo].[VoterView]
AS
SELECT
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

FROM
	dbo.Voter v

