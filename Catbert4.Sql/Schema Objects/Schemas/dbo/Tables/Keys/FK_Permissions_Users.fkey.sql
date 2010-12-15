ALTER TABLE [dbo].[Permissions]
    ADD CONSTRAINT [FK_Permissions_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

