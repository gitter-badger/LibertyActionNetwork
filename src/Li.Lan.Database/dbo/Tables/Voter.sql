CREATE TABLE [dbo].[Voter]
(
	[VoterId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1), 
    [VoterGuid] UNIQUEIDENTIFIER NOT NULL,
	[InsertedBy]          INT              NOT NULL,
    [InsertedOn]          DATETIME         NOT NULL,
    [UpdatedBy]           INT              NOT NULL,
    [UpdatedOn]           DATETIME         NOT NULL,
    [ActiveStatus]        TINYINT          NOT NULL,
	StateVoterId	NVARCHAR(50) NULL,
    FirstName NVARCHAR(50) NOT NULL,
	NickName NVARCHAR(50) NULL,
    LastName NVARCHAR(50) NOT NULL,
    AddressLine1 NVARCHAR(250) NULL,
    AddressLine2 NVARCHAR(250) NULL,
    City NVARCHAR(100) NULL,
    StateCode CHAR(2) NULL,
    ZipCode NVARCHAR(10) NULL,
    PhoneNumber NVARCHAR(30) NULL,
	PhoneNumberType tinyint NULL,
	NewAddressLine1 NVARCHAR(250) NULL,
    NewAddressLine2 NVARCHAR(250) NULL,
    NewCity NVARCHAR(100) NULL,
    NewStateCode CHAR(2) NULL,
    NewZipCode NVARCHAR(10) NULL,
    PhoneNumber2 NVARCHAR(30) NULL,
	PhoneNumber2Type tinyint NULL,
    PrecinctId int NULL,
    FacebookId NVARCHAR(50) NULL,
    TwitterId NVARCHAR(50) NULL, 
    [Email] NVARCHAR(100) NULL, 
    CONSTRAINT [FK_Voter_Precinct] FOREIGN KEY ([PrecinctId]) REFERENCES dbo.Precinct([PrecinctId]) 
)

GO

CREATE INDEX [IX_Voter_LastName] ON [dbo].[Voter] (LastName)
