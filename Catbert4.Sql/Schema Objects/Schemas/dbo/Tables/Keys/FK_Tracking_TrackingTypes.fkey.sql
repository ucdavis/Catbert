ALTER TABLE [dbo].[Tracking]
    ADD CONSTRAINT [FK_Tracking_TrackingTypes] FOREIGN KEY ([TrackingTypeID]) REFERENCES [dbo].[TrackingTypes] ([TrackingTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

