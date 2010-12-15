ALTER TABLE [dbo].[Permissions]
    ADD CONSTRAINT [FK_Permissions_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

