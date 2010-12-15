ALTER TABLE [dbo].[UnitAssociations]
    ADD CONSTRAINT [FK_UnitAssociations_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

