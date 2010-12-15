ALTER TABLE [dbo].[UnitAssociations]
    ADD CONSTRAINT [FK_UnitAssociations_Unit] FOREIGN KEY ([UnitID]) REFERENCES [dbo].[Unit] ([UnitID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

