CREATE TABLE [dbo].[UnitAssociations] (
    [UnitAssociationID] INT IDENTITY (1, 1) NOT NULL,
    [UnitID]            INT NOT NULL,
    [UserID]            INT NOT NULL,
    [ApplicationID]     INT NOT NULL,
    [Inactive]          BIT NOT NULL
);

