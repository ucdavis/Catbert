CREATE TABLE [dbo].[ApplicationRoles] (
    [ApplicationRoleID] INT IDENTITY (1, 1) NOT NULL,
    [ApplicationID]     INT NOT NULL,
    [RoleID]            INT NOT NULL,
    [Level]             INT NULL
);

