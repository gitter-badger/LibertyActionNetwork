CREATE TABLE [dbo].[VoterElection]
(
	[VoterElectionId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[InsertedBy]          INT              NOT NULL,
    [InsertedOn]          DATETIME         NOT NULL,
    [UpdatedBy]           INT              NOT NULL,
    [UpdatedOn]           DATETIME         NOT NULL,
    [ActiveStatus]        TINYINT          NOT NULL, 
    [VoterId] INT NOT NULL, 
    [ElectionId] SMALLINT NOT NULL, 
    [CandidateId] INT NULL, 
    [FromMigrationFlag] TINYINT NULL, 
    CONSTRAINT [FK_VoterElection_Voter] FOREIGN KEY ([VoterId]) REFERENCES [Voter]([VoterId]),
	CONSTRAINT [FK_VoterElection_Election] FOREIGN KEY ([ElectionId]) REFERENCES [Election]([ElectionId]),
	CONSTRAINT [FK_VoterElection_Candidate] FOREIGN KEY ([CandidateId]) REFERENCES [Candidate]([CandidateId]),
)

GO

CREATE INDEX [IX_VoterElection_VoterId] ON [dbo].[VoterElection] ([VoterId])
