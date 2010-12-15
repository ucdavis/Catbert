CREATE TABLE [dbo].[Users] (
    [UserID]     INT              IDENTITY (1, 1) NOT NULL,
    [LoginID]    VARCHAR (10)     NOT NULL,
    [Email]      NVARCHAR (50)    NOT NULL,
    [Phone]      VARCHAR (50)     NULL,
    [FirstName]  NVARCHAR (50)    NULL,
    [LastName]   NVARCHAR (50)    NOT NULL,
    [EmployeeID] VARCHAR (9)      NULL,
    [StudentID]  VARCHAR (9)      NULL,
    [UserImage]  NVARCHAR (50)    NULL,
    [SID]        NVARCHAR (50)    NULL,
    [Inactive]   BIT              NOT NULL,
    [UserKey]    UNIQUEIDENTIFIER NOT NULL
);

