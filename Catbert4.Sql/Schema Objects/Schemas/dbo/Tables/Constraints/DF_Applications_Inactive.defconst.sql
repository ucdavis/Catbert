ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [DF_Applications_Inactive] DEFAULT ((0)) FOR [Inactive];

