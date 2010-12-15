CREATE VIEW [dbo].[UserTracking_V]
AS
SELECT     dbo.UserTracking.TrackingActionDate, dbo.UserTracking.Comments, dbo.UserTracking.UserTrackID, dbo.UserTracking.TrackingUserName, 
                      dbo.Users.FirstName AS SubjectFirstName, dbo.Users.LastName AS SubjectLastName, dbo.Users.EmployeeID AS SubjectEmployeeID, 
                      dbo.TrackingType.TrackingType, logins.LoginID AS SubjectLogin
FROM         dbo.UserTracking INNER JOIN
                      dbo.Users ON dbo.UserTracking.UserID = dbo.Users.UserID INNER JOIN
                      dbo.TrackingType ON dbo.UserTracking.TrackingTypeID = dbo.TrackingType.TrackingTypeID LEFT OUTER JOIN
                          (SELECT     LoginID, UserID
                            FROM          dbo.Login) AS logins ON logins.UserID = dbo.UserTracking.UserID

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[33] 4[28] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "UserTracking"
            Begin Extent = 
               Top = 25
               Left = 377
               Bottom = 179
               Right = 551
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 209
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TrackingType"
            Begin Extent = 
               Top = 111
               Left = 709
               Bottom = 205
               Right = 873
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "logins"
            Begin Extent = 
               Top = 6
               Left = 589
               Bottom = 103
               Right = 749
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'UserTracking_V';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Or = 1350
      End
   End
End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'UserTracking_V';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'UserTracking_V';

