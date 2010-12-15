CREATE TABLE [dbo].[Messages] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [Message]          VARCHAR (MAX) NOT NULL,
    [ApplicationID]    INT           NULL,
    [BeginDisplayDate] DATE          NOT NULL,
    [EndDisplayDate]   DATE          NULL,
    [IsActive]         BIT           NOT NULL
);

