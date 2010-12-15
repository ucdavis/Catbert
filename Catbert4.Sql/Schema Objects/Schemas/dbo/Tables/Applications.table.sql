CREATE TABLE [dbo].[Applications] (
    [ApplicationID]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [Abbr]           NVARCHAR (50)  NULL,
    [Location]       NVARCHAR (256) NULL,
    [IconLocation]   NVARCHAR (256) NULL,
    [Inactive]       BIT            NOT NULL,
    [WebServiceHash] NVARCHAR (100) NULL,
    [Salt]           NVARCHAR (20)  NULL
);

