﻿ALTER TABLE [dbo].[Applications]
    ADD CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([ApplicationID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

