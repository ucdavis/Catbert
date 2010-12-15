ALTER TABLE [dbo].[Permissions]
    ADD CONSTRAINT [FK_Permissions_Applications] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Applications] ([ApplicationID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

