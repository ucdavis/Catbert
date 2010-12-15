ALTER TABLE [dbo].[Unit]
    ADD CONSTRAINT [FK_Unit_Schools] FOREIGN KEY ([SchoolCode]) REFERENCES [dbo].[Schools] ([SchoolCode]) ON DELETE NO ACTION ON UPDATE NO ACTION;

