CREATE TABLE [dbo].[PrecinctPrecinctTag]
(
	[PrecinctPrecinctTagId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
	[ActiveStatus] tinyint CONSTRAINT [DF_PrecinctPrecinctTag_ActiveStatus] DEFAULT ((1)) NOT NULL,
    [PrecinctId] INT NOT NULL, 
    [PrecinctTagId] INT NOT NULL, 
    [ActiveFrom] DATETIME NOT NULL, 
    [ActiveTo] DATETIME NOT NULL, 
    CONSTRAINT [FK_PrecinctPrecinctTag_Precinct] FOREIGN KEY ([PrecinctId]) REFERENCES dbo.Precinct([PrecinctId]), 
    CONSTRAINT [FK_PrecinctPrecinctTag_PrecinctTag] FOREIGN KEY ([PrecinctTagId]) REFERENCES dbo.PrecinctTag([PrecinctTagId])
)
