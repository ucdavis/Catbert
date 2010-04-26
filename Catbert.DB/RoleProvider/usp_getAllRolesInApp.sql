﻿IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_getAllRolesInApp')
	BEGIN
		DROP  Procedure  usp_getAllRolesInApp
	END

GO

CREATE Procedure usp_getAllRolesInApp
	-- Add the parameters for the stored procedure here
	@AppName nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     Roles.Role
FROM         Applications INNER JOIN
                      Permissions ON Applications.ApplicationID = Permissions.ApplicationID INNER JOIN
                      Roles ON Permissions.RoleID = Roles.RoleID
WHERE     (Applications.Name = @AppName) AND (Applications.Inactive = 0) AND (Permissions.Inactive = 0) AND (Roles.Inactive = 0)
END
