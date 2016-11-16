CREATE TABLE [dbo].[State] (
    [StateId]     TINYINT          NOT NULL,
    [StateGuid]   UNIQUEIDENTIFIER NOT NULL,
    [Code]        CHAR (2)         NOT NULL,
    [Description] NVARCHAR (50)    NOT NULL,
    PRIMARY KEY CLUSTERED ([StateId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_State_Code]
    ON [dbo].[State]([Code] ASC);

