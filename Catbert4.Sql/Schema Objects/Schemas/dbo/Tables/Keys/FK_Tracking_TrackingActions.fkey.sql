ALTER TABLE [dbo].[Tracking]
    ADD CONSTRAINT [FK_Tracking_TrackingActions] FOREIGN KEY ([TrackingActionID]) REFERENCES [dbo].[TrackingActions] ([TrackingActionID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

