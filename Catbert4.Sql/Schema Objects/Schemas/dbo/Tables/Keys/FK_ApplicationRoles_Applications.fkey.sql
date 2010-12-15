ALTER TABLE [dbo].[ApplicationRoles]
    ADD CONSTRAINT [FK_ApplicationRoles_Applications] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Applications] ([ApplicationID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

