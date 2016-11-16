CREATE TABLE [dbo].[VoterIssueTag]
(
	[VoterIssueTagId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [InsertedBy]          INT              NOT NULL,
    [InsertedOn]          DATETIME         NOT NULL,
    [UpdatedBy]           INT              NOT NULL,
    [UpdatedOn]           DATETIME         NOT NULL,
    [ActiveStatus]        TINYINT          NOT NULL, 
    [VoterId] INT NOT NULL, 
    [IssueTagId] SMALLINT NOT NULL, 
    [Priority] TINYINT NOT NULL, 
    [FromMigrationFlag] TINYINT NULL, 
    CONSTRAINT [FK_VoterIssueTag_Voter] FOREIGN KEY ([VoterId]) REFERENCES dbo.Voter([VoterId]), 
    CONSTRAINT [FK_VoterIssueTag_IssueTag] FOREIGN KEY ([IssueTagId]) REFERENCES dbo.IssueTag([IssueTagId]) ,
	
)
