ALTER TABLE [dbo].[Messages]
    ADD CONSTRAINT [DF_Messages_IsActive] DEFAULT ((1)) FOR [IsActive];

