ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_Inactive] DEFAULT ((0)) FOR [Inactive];

