CREATE TABLE [dbo].[PrecinctTag]
(
	[PrecinctTagId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [PrecinctTagTypeId] TINYINT NOT NULL, 
    [Description] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_PrecinctTag_PrecinctTagType] FOREIGN KEY ([PrecinctTagTypeId]) REFERENCES dbo.PrecinctTagType([PrecinctTagTypeId])
)
