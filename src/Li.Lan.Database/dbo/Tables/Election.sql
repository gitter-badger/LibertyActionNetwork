CREATE TABLE [dbo].[Election]
(
	[ElectionId] SMALLINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [ElectionDate] DATE NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [ElectionTypeId] TINYINT NOT NULL
)
