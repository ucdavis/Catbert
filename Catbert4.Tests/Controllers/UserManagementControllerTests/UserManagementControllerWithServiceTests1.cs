using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Catbert4.Controllers;
using Catbert4.Core.Domain;
using Catbert4.Helpers;
using Catbert4.Models;
using Catbert4.Services;
using Catbert4.Services.UserManagement;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catbert4.Tests.Core.Extensions;
using MvcContrib.TestHelper;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Attributes;
using Rhino;
using Rhino.Mocks;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region GetByApplication Tests

        [TestMethod]
        public void TestGetByApplication1()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            var users = new List<User>();
            for (int i = 0; i < 5; i++)
            {
                users.Add(CreateValidEntities.User(i+1));
                users[i].UnitAssociations.Add(CreateValidEntities.UnitAssociation(i+1));
                users[i].UnitAssociations[0].Unit = UnitRepository.GetNullableById(i+1);
            }
            ControllerRecordFakes.FakeUsers(0, UserRepository, users);

            var permissions = new List<Permission>();
            for (int i = 0; i < 5; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i+1));
                permissions[i].User = UserRepository.GetNullableById(i+1);
                permissions[i].Application = ApplicationRepository.Queryable.First();
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);
            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 5; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i+1));
                unitAssociations[i].User = UserRepository.GetNullableById(i+1);
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);

            #endregion Arrange

            #region Act
            var result = UserService.GetByApplication("Name1", "UserName");
            //var allowedUnitIds = UnitService.GetVisibleByUser("Name1", "UserName").ToList().Select(x => x.Id).ToList();

            ////Get everyone with perms, possibly filtered by role and unit
            //var usersWithPermissions = from p in PermissionRepository.Queryable
            //                           join u in UnitAssociationRepository.Queryable on
            //                               new { User = p.User.Id, App = p.Application.Id }
            //                               equals new { User = u.User.Id, App = u.Application.Id }
            //                           where p.Application.Name == "Name1"
            //                           where p.User.UnitAssociations.Any(a => allowedUnitIds.Contains(a.Unit.Id))
            //                           select new { Permissions = p, UnitAssociations = u };
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("LoginId2",result.ElementAt(0).LoginId);
            Assert.AreEqual("LoginId4", result.ElementAt(1).LoginId);
            #endregion Assert		
        }

        [TestMethod]
        public void TestGetByApplication2()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();
            var users = new List<User>();
            //User 1 has Unit 1, unit 3 -- no match
            users.Add(CreateValidEntities.User(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            users[0].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[0].UnitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            //User2 has unit1, 2, 3, and 4 //Match
            users.Add(CreateValidEntities.User(2));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(4));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(5));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(6));
            users[1].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[1].UnitAssociations[1].Unit = UnitRepository.GetNullableById(2);
            users[1].UnitAssociations[2].Unit = UnitRepository.GetNullableById(3);
            users[1].UnitAssociations[3].Unit = UnitRepository.GetNullableById(4);

            //User3 has unit 4 //Match
            users.Add(CreateValidEntities.User(3));
            users[2].UnitAssociations.Add(CreateValidEntities.UnitAssociation(7));
            users[2].UnitAssociations[0].Unit = UnitRepository.GetNullableById(4);

            //user4 has unit2 //match
            users.Add(CreateValidEntities.User(4));
            users[3].UnitAssociations.Add(CreateValidEntities.UnitAssociation(8));
            users[3].UnitAssociations[0].Unit = UnitRepository.GetNullableById(2);

            //user5 has unit 5 and 1, no match
            users.Add(CreateValidEntities.User(5));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(9));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(10));
            users[4].UnitAssociations[0].Unit = UnitRepository.GetNullableById(5);
            users[4].UnitAssociations[1].Unit = UnitRepository.GetNullableById(1);

            ControllerRecordFakes.FakeUsers(0, UserRepository, users);

            var permissions = new List<Permission>();
            var count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    count++;
                    permissions.Add(CreateValidEntities.Permission(count));
                    permissions[count-1].User = UserRepository.GetNullableById(x + 1);
                    permissions[count-1].Application = ApplicationRepository.Queryable.First();
                    permissions[count-1].Role = RoleRepository.GetNullableById(x + 1);
                }
               
            }
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);
            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 10; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
            }
            unitAssociations[0].User = UserRepository.GetNullableById(1);
            unitAssociations[0].Application = ApplicationRepository.Queryable.First();
            unitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[1].User = UserRepository.GetNullableById(1);
            unitAssociations[1].Application = ApplicationRepository.Queryable.First();
            unitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            unitAssociations[2].User = UserRepository.GetNullableById(2);
            unitAssociations[2].Application = ApplicationRepository.Queryable.First();
            unitAssociations[2].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[3].User = UserRepository.GetNullableById(2);
            unitAssociations[3].Application = ApplicationRepository.Queryable.First();
            unitAssociations[3].Unit = UnitRepository.GetNullableById(2);
            unitAssociations[4].User = UserRepository.GetNullableById(2);
            unitAssociations[4].Application = ApplicationRepository.Queryable.First();
            unitAssociations[4].Unit = UnitRepository.GetNullableById(3);
            unitAssociations[5].User = UserRepository.GetNullableById(2);
            unitAssociations[5].Application = ApplicationRepository.Queryable.First();
            unitAssociations[5].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[6].User = UserRepository.GetNullableById(3);
            unitAssociations[6].Application = ApplicationRepository.Queryable.First();
            unitAssociations[6].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[7].User = UserRepository.GetNullableById(4);
            unitAssociations[7].Application = ApplicationRepository.Queryable.First();
            unitAssociations[7].Unit = UnitRepository.GetNullableById(2);

            unitAssociations[8].User = UserRepository.GetNullableById(5);
            unitAssociations[8].Application = ApplicationRepository.Queryable.First();
            unitAssociations[8].Unit = UnitRepository.GetNullableById(5);
            unitAssociations[9].User = UserRepository.GetNullableById(5);
            unitAssociations[9].Application = ApplicationRepository.Queryable.First();
            unitAssociations[9].Unit = UnitRepository.GetNullableById(1);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);

            #endregion Arrange

            #region Act
            var result = UserService.GetByApplication("Name1", "UserName");
            //var allowedUnitIds = UnitService.GetVisibleByUser("Name1", "UserName").ToList().Select(x => x.Id).ToList();

            ////Get everyone with perms, possibly filtered by role and unit
            //var usersWithPermissions = from p in PermissionRepository.Queryable
            //                           join u in UnitAssociationRepository.Queryable on
            //                               new { User = p.User.Id, App = p.Application.Id }
            //                               equals new { User = u.User.Id, App = u.Application.Id }
            //                           where p.Application.Name == "Name1"
            //                           where p.User.UnitAssociations.Any(a => allowedUnitIds.Contains(a.Unit.Id))
            //                           select new { Permissions = p, UnitAssociations = u };
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("LoginId2", result.ElementAt(0).LoginId);
            Assert.AreEqual("LoginId3", result.ElementAt(1).LoginId);
            Assert.AreEqual("LoginId4", result.ElementAt(2).LoginId);
            #endregion Assert
        }

        [TestMethod]
        public void TestGetByApplication3()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();
            var users = new List<User>();
            //User 1 has Unit 1, unit 3 -- no match
            users.Add(CreateValidEntities.User(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            users[0].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[0].UnitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            //User2 has unit1, 2, 3, and 4 //Match
            users.Add(CreateValidEntities.User(2));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(4));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(5));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(6));
            users[1].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[1].UnitAssociations[1].Unit = UnitRepository.GetNullableById(2);
            users[1].UnitAssociations[2].Unit = UnitRepository.GetNullableById(3);
            users[1].UnitAssociations[3].Unit = UnitRepository.GetNullableById(4);

            //User3 has unit 4 //Match
            users.Add(CreateValidEntities.User(3));
            users[2].UnitAssociations.Add(CreateValidEntities.UnitAssociation(7));
            users[2].UnitAssociations[0].Unit = UnitRepository.GetNullableById(4);

            //user4 has unit2 //match
            users.Add(CreateValidEntities.User(4));
            users[3].UnitAssociations.Add(CreateValidEntities.UnitAssociation(8));
            users[3].UnitAssociations[0].Unit = UnitRepository.GetNullableById(2);

            //user5 has unit 5 and 1, no match
            users.Add(CreateValidEntities.User(5));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(9));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(10));
            users[4].UnitAssociations[0].Unit = UnitRepository.GetNullableById(5);
            users[4].UnitAssociations[1].Unit = UnitRepository.GetNullableById(1);

            ControllerRecordFakes.FakeUsers(0, UserRepository, users);

            var permissions = new List<Permission>();
            var count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    count++;
                    permissions.Add(CreateValidEntities.Permission(count));
                    permissions[count - 1].User = UserRepository.GetNullableById(x + 1);
                    permissions[count - 1].Application = ApplicationRepository.Queryable.First();
                    permissions[count - 1].Role = RoleRepository.GetNullableById(x + 1);
                }

            }
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);
            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 10; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
            }
            unitAssociations[0].User = UserRepository.GetNullableById(1);
            unitAssociations[0].Application = ApplicationRepository.Queryable.First();
            unitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[1].User = UserRepository.GetNullableById(1);
            unitAssociations[1].Application = ApplicationRepository.Queryable.First();
            unitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            unitAssociations[2].User = UserRepository.GetNullableById(2);
            unitAssociations[2].Application = ApplicationRepository.Queryable.First();
            unitAssociations[2].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[3].User = UserRepository.GetNullableById(2);
            unitAssociations[3].Application = ApplicationRepository.Queryable.First();
            unitAssociations[3].Unit = UnitRepository.GetNullableById(2);
            unitAssociations[4].User = UserRepository.GetNullableById(2);
            unitAssociations[4].Application = ApplicationRepository.GetNullableById(2); //No Match, but others do for this user
            unitAssociations[4].Unit = UnitRepository.GetNullableById(3);
            unitAssociations[5].User = UserRepository.GetNullableById(2);
            unitAssociations[5].Application = ApplicationRepository.Queryable.First();
            unitAssociations[5].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[6].User = UserRepository.GetNullableById(3);
            unitAssociations[6].Application = ApplicationRepository.GetNullableById(2); //No Match
            unitAssociations[6].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[7].User = UserRepository.GetNullableById(4);
            unitAssociations[7].Application = ApplicationRepository.Queryable.First();
            unitAssociations[7].Unit = UnitRepository.GetNullableById(2);

            unitAssociations[8].User = UserRepository.GetNullableById(5);
            unitAssociations[8].Application = ApplicationRepository.Queryable.First();
            unitAssociations[8].Unit = UnitRepository.GetNullableById(5);
            unitAssociations[9].User = UserRepository.GetNullableById(5);
            unitAssociations[9].Application = ApplicationRepository.Queryable.First();
            unitAssociations[9].Unit = UnitRepository.GetNullableById(1);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);

            #endregion Arrange

            #region Act
            var result = UserService.GetByApplication("Name1", "UserName");
            //var allowedUnitIds = UnitService.GetVisibleByUser("Name1", "UserName").ToList().Select(x => x.Id).ToList();

            ////Get everyone with perms, possibly filtered by role and unit
            //var usersWithPermissions = from p in PermissionRepository.Queryable
            //                           join u in UnitAssociationRepository.Queryable on
            //                               new { User = p.User.Id, App = p.Application.Id }
            //                               equals new { User = u.User.Id, App = u.Application.Id }
            //                           where p.Application.Name == "Name1"
            //                           where p.User.UnitAssociations.Any(a => allowedUnitIds.Contains(a.Unit.Id))
            //                           select new { Permissions = p, UnitAssociations = u };
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("LoginId2", result.ElementAt(0).LoginId);
            //Assert.AreEqual("LoginId3", result.ElementAt(1).LoginId);
            Assert.AreEqual("LoginId4", result.ElementAt(1).LoginId);
            #endregion Assert
        }

        [TestMethod]
        public void TestGetByApplication4()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();
            var users = new List<User>();
            //User 1 has Unit 1, unit 3 -- no match
            users.Add(CreateValidEntities.User(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            users[0].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[0].UnitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            //User2 has unit1, 2, 3, and 4 //Match
            users.Add(CreateValidEntities.User(2));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(4));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(5));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(6));
            users[1].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[1].UnitAssociations[1].Unit = UnitRepository.GetNullableById(2);
            users[1].UnitAssociations[2].Unit = UnitRepository.GetNullableById(3);
            users[1].UnitAssociations[3].Unit = UnitRepository.GetNullableById(4);

            //User3 has unit 4 //Match
            users.Add(CreateValidEntities.User(3));
            users[2].UnitAssociations.Add(CreateValidEntities.UnitAssociation(7));
            users[2].UnitAssociations[0].Unit = UnitRepository.GetNullableById(4);

            //user4 has unit2 //match
            users.Add(CreateValidEntities.User(4));
            users[3].UnitAssociations.Add(CreateValidEntities.UnitAssociation(8));
            users[3].UnitAssociations[0].Unit = UnitRepository.GetNullableById(2);

            //user5 has unit 5 and 1, no match
            users.Add(CreateValidEntities.User(5));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(9));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(10));
            users[4].UnitAssociations[0].Unit = UnitRepository.GetNullableById(5);
            users[4].UnitAssociations[1].Unit = UnitRepository.GetNullableById(1);

            ControllerRecordFakes.FakeUsers(0, UserRepository, users);

            var permissions = new List<Permission>();
            var count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    
                    if (x != 1)
                    {
                        count++;
                        permissions.Add(CreateValidEntities.Permission(count));
                        permissions[count - 1].User = UserRepository.GetNullableById(x + 1);
                        permissions[count - 1].Application = ApplicationRepository.Queryable.First();
                        permissions[count - 1].Role = RoleRepository.GetNullableById(x + 1);
                    }
                }

            }
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);
            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 10; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
            }
            unitAssociations[0].User = UserRepository.GetNullableById(1);
            unitAssociations[0].Application = ApplicationRepository.Queryable.First();
            unitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[1].User = UserRepository.GetNullableById(1);
            unitAssociations[1].Application = ApplicationRepository.Queryable.First();
            unitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            unitAssociations[2].User = UserRepository.GetNullableById(2);
            unitAssociations[2].Application = ApplicationRepository.Queryable.First();
            unitAssociations[2].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[3].User = UserRepository.GetNullableById(2);
            unitAssociations[3].Application = ApplicationRepository.Queryable.First();
            unitAssociations[3].Unit = UnitRepository.GetNullableById(2);
            unitAssociations[4].User = UserRepository.GetNullableById(2);
            unitAssociations[4].Application = ApplicationRepository.Queryable.First();
            unitAssociations[4].Unit = UnitRepository.GetNullableById(3);
            unitAssociations[5].User = UserRepository.GetNullableById(2);
            unitAssociations[5].Application = ApplicationRepository.Queryable.First();
            unitAssociations[5].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[6].User = UserRepository.GetNullableById(3);
            unitAssociations[6].Application = ApplicationRepository.Queryable.First();
            unitAssociations[6].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[7].User = UserRepository.GetNullableById(4);
            unitAssociations[7].Application = ApplicationRepository.Queryable.First();
            unitAssociations[7].Unit = UnitRepository.GetNullableById(2);

            unitAssociations[8].User = UserRepository.GetNullableById(5);
            unitAssociations[8].Application = ApplicationRepository.Queryable.First();
            unitAssociations[8].Unit = UnitRepository.GetNullableById(5);
            unitAssociations[9].User = UserRepository.GetNullableById(5);
            unitAssociations[9].Application = ApplicationRepository.Queryable.First();
            unitAssociations[9].Unit = UnitRepository.GetNullableById(1);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);

            #endregion Arrange

            #region Act
            var result = UserService.GetByApplication("Name1", "UserName");
            //var allowedUnitIds = UnitService.GetVisibleByUser("Name1", "UserName").ToList().Select(x => x.Id).ToList();

            ////Get everyone with perms, possibly filtered by role and unit
            //var usersWithPermissions = from p in PermissionRepository.Queryable
            //                           join u in UnitAssociationRepository.Queryable on
            //                               new { User = p.User.Id, App = p.Application.Id }
            //                               equals new { User = u.User.Id, App = u.Application.Id }
            //                           where p.Application.Name == "Name1"
            //                           where p.User.UnitAssociations.Any(a => allowedUnitIds.Contains(a.Unit.Id))
            //                           select new { Permissions = p, UnitAssociations = u };
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            //Assert.AreEqual("LoginId2", result.ElementAt(0).LoginId); //No permissions
            Assert.AreEqual("LoginId3", result.ElementAt(0).LoginId);
            Assert.AreEqual("LoginId4", result.ElementAt(1).LoginId);
            #endregion Assert
        }

        [TestMethod]
        public void TestGetByApplication5()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();
            var users = new List<User>();
            //User 1 has Unit 1, unit 3 -- no match
            users.Add(CreateValidEntities.User(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            users[0].UnitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            users[0].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[0].UnitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            //User2 has unit1, 2, 3, and 4 //Match
            users.Add(CreateValidEntities.User(2));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(4));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(5));
            users[1].UnitAssociations.Add(CreateValidEntities.UnitAssociation(6));
            users[1].UnitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            users[1].UnitAssociations[1].Unit = UnitRepository.GetNullableById(2);
            users[1].UnitAssociations[2].Unit = UnitRepository.GetNullableById(3);
            users[1].UnitAssociations[3].Unit = UnitRepository.GetNullableById(4);

            //User3 has unit 4 //Match
            users.Add(CreateValidEntities.User(3));
            users[2].UnitAssociations.Add(CreateValidEntities.UnitAssociation(7));
            users[2].UnitAssociations[0].Unit = UnitRepository.GetNullableById(4);

            //user4 has unit2 //match
            users.Add(CreateValidEntities.User(4));
            users[3].UnitAssociations.Add(CreateValidEntities.UnitAssociation(8));
            users[3].UnitAssociations[0].Unit = UnitRepository.GetNullableById(2);

            //user5 has unit 5 and 1, no match
            users.Add(CreateValidEntities.User(5));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(9));
            users[4].UnitAssociations.Add(CreateValidEntities.UnitAssociation(10));
            users[4].UnitAssociations[0].Unit = UnitRepository.GetNullableById(5);
            users[4].UnitAssociations[1].Unit = UnitRepository.GetNullableById(1);

            ControllerRecordFakes.FakeUsers(0, UserRepository, users);

            var permissions = new List<Permission>();
            var count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    count++;
                    permissions.Add(CreateValidEntities.Permission(count));
                    permissions[count - 1].User = UserRepository.GetNullableById(x + 1);
                    permissions[count - 1].Application = ApplicationRepository.Queryable.First();
                    permissions[count - 1].Role = RoleRepository.GetNullableById(x + 1);
                }

            }
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);
            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 10; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
            }
            unitAssociations[0].User = UserRepository.GetNullableById(1);
            unitAssociations[0].Application = ApplicationRepository.Queryable.First();
            unitAssociations[0].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[1].User = UserRepository.GetNullableById(1);
            unitAssociations[1].Application = ApplicationRepository.Queryable.First();
            unitAssociations[1].Unit = UnitRepository.GetNullableById(3);

            unitAssociations[2].User = UserRepository.GetNullableById(2);
            unitAssociations[2].Application = ApplicationRepository.Queryable.First();
            unitAssociations[2].Unit = UnitRepository.GetNullableById(1);
            unitAssociations[3].User = UserRepository.GetNullableById(2);
            unitAssociations[3].Application = ApplicationRepository.Queryable.First();
            unitAssociations[3].Unit = UnitRepository.GetNullableById(2);
            unitAssociations[4].User = UserRepository.GetNullableById(2);
            unitAssociations[4].Application = ApplicationRepository.Queryable.First();
            unitAssociations[4].Unit = UnitRepository.GetNullableById(3);
            unitAssociations[5].User = UserRepository.GetNullableById(2);
            unitAssociations[5].Application = ApplicationRepository.Queryable.First();
            unitAssociations[5].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[6].User = UserRepository.GetNullableById(3);
            unitAssociations[6].Application = ApplicationRepository.Queryable.First();
            unitAssociations[6].Unit = UnitRepository.GetNullableById(4);

            unitAssociations[7].User = UserRepository.GetNullableById(4);
            unitAssociations[7].Application = ApplicationRepository.Queryable.First();
            unitAssociations[7].Unit = UnitRepository.GetNullableById(2);

            unitAssociations[8].User = UserRepository.GetNullableById(5);
            unitAssociations[8].Application = ApplicationRepository.Queryable.First();
            unitAssociations[8].Unit = UnitRepository.GetNullableById(5);
            unitAssociations[9].User = UserRepository.GetNullableById(5);
            unitAssociations[9].Application = ApplicationRepository.Queryable.First();
            unitAssociations[9].Unit = UnitRepository.GetNullableById(1);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);

            #endregion Arrange

            #region Act
            var result = UserService.GetByApplication("Name1", "UserName", null, "F4");
            //var allowedUnitIds = UnitService.GetVisibleByUser("Name1", "UserName").ToList().Select(x => x.Id).ToList();

            ////Get everyone with perms, possibly filtered by role and unit
            //var usersWithPermissions = from p in PermissionRepository.Queryable
            //                           join u in UnitAssociationRepository.Queryable on
            //                               new { User = p.User.Id, App = p.Application.Id }
            //                               equals new { User = u.User.Id, App = u.Application.Id }
            //                           where p.Application.Name == "Name1"
            //                           where p.User.UnitAssociations.Any(a => allowedUnitIds.Contains(a.Unit.Id))
            //                           select new { Permissions = p, UnitAssociations = u };
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("LoginId2", result.ElementAt(0).LoginId);
            Assert.AreEqual("LoginId3", result.ElementAt(1).LoginId);
            //Assert.AreEqual("LoginId4", result.ElementAt(2).LoginId);
            #endregion Assert
        }

        #endregion GetByApplication Tests
    }
}
