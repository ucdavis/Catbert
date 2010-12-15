ALTER TABLE [dbo].[Permissions]
    ADD CONSTRAINT [DF_Permissions_Inactive] DEFAULT ((0)) FOR [Inactive];

