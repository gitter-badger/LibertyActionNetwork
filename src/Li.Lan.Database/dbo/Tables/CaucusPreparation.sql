CREATE TABLE [dbo].[CaucusPreparation]
(
	[CaucusPreparationId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[InsertedBy]          INT              NOT NULL,
    [InsertedOn]          DATETIME         NOT NULL,
    [UpdatedBy]           INT              NOT NULL,
    [UpdatedOn]           DATETIME         NOT NULL,
    [ActiveStatus]        TINYINT          NOT NULL, 
    [VoterId] INT NOT NULL, 
	[ElectionId] SMALLINT NOT NULL,
    [CallDispositionId] TINYINT NOT NULL,
	[IsAttending] TINYINT NULL, 
    [IsDelegate] TINYINT NULL, 
    [IsCentralCommittee] TINYINT NULL, 
    [IsVolunteer] TINYINT NULL, 
    [Note] NVARCHAR(1000) NULL, 
    CONSTRAINT [FK_CaucusPreparation_Voter] FOREIGN KEY ([VoterId]) REFERENCES dbo.Voter([VoterId]),
    CONSTRAINT [FK_CaucusPreparation_Election] FOREIGN KEY ([ElectionId]) REFERENCES dbo.Election([ElectionId]) 
)
