CREATE TABLE [dbo].[Log] (
    [LogId]              INT              IDENTITY (1, 1) NOT NULL,
    [LogGuid]            UNIQUEIDENTIFIER NOT NULL,
    [InsertedOnUtc]      DATETIME         NOT NULL,
    [StoredOnUtc]        DATETIME         NULL,
    [CreatedOnUtc]       DATETIME         NULL,
    [ApplicationVersion] CHAR (14)        NULL,
    [UserId]             INT              NULL,
    [SessionId]          UNIQUEIDENTIFIER NULL,
    [ActionId]           UNIQUEIDENTIFIER NULL,
    [Tag]                NVARCHAR (200)   NULL,
    [Category]           NVARCHAR (200)   NULL,
    [SubCategory]        NVARCHAR (200)   NULL,
    [Message]            NVARCHAR (4000)  NULL,
    [LogType]            TINYINT          NULL,
    [UserHostAddress]    VARCHAR (50)     NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([LogId] ASC)
);

