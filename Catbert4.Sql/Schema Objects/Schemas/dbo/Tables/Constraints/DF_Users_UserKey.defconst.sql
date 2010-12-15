ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [DF_Users_UserKey] DEFAULT (newid()) FOR [UserKey];

