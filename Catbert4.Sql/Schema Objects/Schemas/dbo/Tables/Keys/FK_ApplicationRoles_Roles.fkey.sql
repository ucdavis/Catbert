ALTER TABLE [dbo].[ApplicationRoles]
    ADD CONSTRAINT [FK_ApplicationRoles_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

