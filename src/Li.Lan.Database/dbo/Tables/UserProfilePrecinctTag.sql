CREATE TABLE [dbo].[UserProfilePrecinctTag]
(
	[UserProfilePrecinctTagId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[InsertedBy]               INT NOT NULL,
    [InsertedOn]               DATETIME NOT NULL,
    [UpdatedBy]                INT NOT NULL,
    [UpdatedOn]                DATETIME NOT NULL,
    [ActiveStatus]             TINYINT NOT NULL, 
    [UserId] INT NOT NULL, 
    [PrecinctTagId] INT NOT NULL, 
    CONSTRAINT [FK_UserProfilePrecinctTag_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [UserProfile]([UserId]), 
    CONSTRAINT [FK_UserProfilePrecinctTag_PrecinctTag] FOREIGN KEY ([PrecinctTagId]) REFERENCES [PrecinctTag]([PrecinctTagId]),
)
