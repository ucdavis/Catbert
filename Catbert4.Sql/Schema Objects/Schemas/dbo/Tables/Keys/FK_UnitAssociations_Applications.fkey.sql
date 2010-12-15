ALTER TABLE [dbo].[UnitAssociations]
    ADD CONSTRAINT [FK_UnitAssociations_Applications] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Applications] ([ApplicationID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

