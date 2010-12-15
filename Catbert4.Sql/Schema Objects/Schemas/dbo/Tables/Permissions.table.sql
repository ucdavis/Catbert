CREATE TABLE [dbo].[Permissions] (
    [PermissionID]  INT IDENTITY (1, 1) NOT NULL,
    [UserID]        INT NOT NULL,
    [ApplicationID] INT NULL,
    [RoleID]        INT NULL,
    [Inactive]      BIT NOT NULL
);

