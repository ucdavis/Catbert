CREATE VIEW [dbo].[PermissionTracking_V]
AS
SELECT     dbo.Permissions.PermissionID, dbo.Applications.Name, dbo.Roles.Role, dbo.Permissions.Inactive, dbo.Users.FirstName, dbo.Users.LastName, dbo.Login.LoginID, 
                      dbo.PermissionTracking.TrackingTypeID, dbo.PermissionTracking.TrackingUserName, dbo.PermissionTracking.TrackingActionDate, 
                      dbo.PermissionTracking.Comments
FROM         dbo.Permissions INNER JOIN
                      dbo.PermissionTracking ON dbo.Permissions.PermissionID = dbo.PermissionTracking.PermissionID INNER JOIN
                      dbo.Users ON dbo.Permissions.UserID = dbo.Users.UserID INNER JOIN
                      dbo.Applications ON dbo.Permissions.ApplicationID = dbo.Applications.ApplicationID INNER JOIN
                      dbo.Roles ON dbo.Permissions.RoleID = dbo.Roles.RoleID INNER JOIN
                      dbo.Login ON dbo.Users.UserID = dbo.Login.UserID

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
         Begin Table = "Permissions"
            Begin Extent = 
               Top = 20
               Left = 547
               Bottom = 165
               Right = 707
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PermissionTracking"
            Begin Extent = 
               Top = 8
               Left = 219
               Bottom = 256
               Right = 409
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Users"
            Begin Extent = 
               Top = 28
               Left = 918
               Bottom = 235
               Right = 1078
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Applications"
            Begin Extent = 
               Top = 206
               Left = 682
               Bottom = 323
               Right = 852
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Roles"
            Begin Extent = 
               Top = 212
               Left = 435
               Bottom = 328
               Right = 595
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Login"
            Begin Extent = 
               Top = 219
               Left = 1090
               Bottom = 306
               Right = 1250
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
      Begin ColumnWidths = 11
         Width = 284
         Width = 1200', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PermissionTracking_V';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
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
         Or = 1350
         Or = 1350
      End
   End
End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PermissionTracking_V';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PermissionTracking_V';

