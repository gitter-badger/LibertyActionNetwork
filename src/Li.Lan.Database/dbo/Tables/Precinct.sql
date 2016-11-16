CREATE TABLE [dbo].[Precinct]
(
	[PrecinctId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [ActiveStatus]             TINYINT          CONSTRAINT [DF_Precinct_ActiveStatus] DEFAULT ((1)) NOT NULL,
	[Code] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    [County] NVARCHAR(50) NULL
)
