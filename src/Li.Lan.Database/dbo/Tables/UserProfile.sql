CREATE TABLE [dbo].[UserProfile] (
    [UserId]                   INT              IDENTITY (1, 1) NOT NULL,
    [UserGuid]                 UNIQUEIDENTIFIER CONSTRAINT [DF_UserProfile_UserGuid] DEFAULT (newid()) NOT NULL,
    [InsertedBy]               INT              CONSTRAINT [DF_UserProfile_InsertedBy] DEFAULT ((0)) NOT NULL,
    [InsertedOn]               DATETIME         CONSTRAINT [DF_UserProfile_InsertedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedBy]                INT              CONSTRAINT [DF_UserProfile_UpdatedBy] DEFAULT ((0)) NOT NULL,
    [UpdatedOn]                DATETIME         CONSTRAINT [DF_UserProfile_UpdatedOn] DEFAULT (getutcdate()) NOT NULL,
    [ActiveStatus]             TINYINT          CONSTRAINT [DF_UserProfile_ActiveStatus] DEFAULT ((1)) NOT NULL,
    [UserName]                 NVARCHAR (250)   NOT NULL,
    [TwitterAccessToken]       NVARCHAR (250)   NULL,
    [TwitterAccessTokenSecret] NVARCHAR (250)   NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO

CREATE UNIQUE INDEX [IX_UserProfile_UserName] ON [dbo].[UserProfile] ([UserName])
