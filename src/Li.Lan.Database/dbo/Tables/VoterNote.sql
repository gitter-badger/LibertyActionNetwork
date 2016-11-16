CREATE TABLE [dbo].[VoterNote]
(
	[VoterNoteId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[InsertedBy]          INT              NOT NULL,
    [InsertedOn]          DATETIME         NOT NULL,
    [UpdatedBy]           INT              NOT NULL,
    [UpdatedOn]           DATETIME         NOT NULL,
    [ActiveStatus]        TINYINT          NOT NULL, 
    [VoterId] INT NOT NULL, 
	[Note] NVARCHAR(1000) NOT NULL,
    CONSTRAINT [FK_VoterNote_Voter] FOREIGN KEY ([VoterId]) REFERENCES [Voter]([VoterId])
)
