CREATE TABLE [dbo].[VoterCandidatePreference]
(
	[VoterCandidatePreferenceId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[InsertedBy]          INT              NOT NULL,
    [InsertedOn]          DATETIME         NOT NULL,
    [UpdatedBy]           INT              NOT NULL,
    [UpdatedOn]           DATETIME         NOT NULL,
    [ActiveStatus]        TINYINT          NOT NULL, 
    [VoterId] INT NOT NULL, 
    [CandidateId] INT NOT NULL, 
    [Priority] TINYINT NOT NULL, 
    [SupportLevel] TINYINT NOT NULL, 
    [FromMigrationFlag] TINYINT NULL, 
    CONSTRAINT [FK_VoterCandidatePreference_Voter] FOREIGN KEY ([VoterId]) REFERENCES dbo.Voter([VoterId]), 
    CONSTRAINT [FK_VoterCandidatePreference_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES dbo.Candidate([CandidateId]) ,
	
)
