CREATE TABLE [dbo].[Tracking] (
    [TrackingID]         INT            IDENTITY (1, 1) NOT NULL,
    [TrackingTypeID]     INT            NOT NULL,
    [TrackingActionID]   INT            NOT NULL,
    [TrackingUserName]   VARCHAR (10)   NOT NULL,
    [TrackingActionDate] DATETIME       NOT NULL,
    [Comments]           NVARCHAR (MAX) NULL
);

