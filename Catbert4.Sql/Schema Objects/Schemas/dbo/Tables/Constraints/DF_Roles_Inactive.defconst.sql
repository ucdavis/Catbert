ALTER TABLE [dbo].[Roles]
    ADD CONSTRAINT [DF_Roles_Inactive] DEFAULT ((0)) FOR [Inactive];

