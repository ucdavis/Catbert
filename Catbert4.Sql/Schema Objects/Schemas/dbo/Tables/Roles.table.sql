CREATE TABLE [dbo].[Roles] (
    [RoleID]   INT           IDENTITY (1, 1) NOT NULL,
    [Role]     NVARCHAR (50) NOT NULL,
    [Inactive] BIT           NOT NULL
);

