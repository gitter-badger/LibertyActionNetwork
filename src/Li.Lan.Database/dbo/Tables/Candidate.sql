CREATE TABLE [dbo].[Candidate]
(
	[CandidateId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [InsertedOn] DATETIME NOT NULL, 
    [ElectionId] SMALLINT NOT NULL, 
	[PositionId] INT NULL,
    [Name] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [FK_Candidate_Election] FOREIGN KEY ([ElectionId]) REFERENCES [Election]([ElectionId]),
    CONSTRAINT [FK_Candidate_Position] FOREIGN KEY ([PositionId]) REFERENCES [Position]([PositionId])
)
