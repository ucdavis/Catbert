using System;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using Catbert4.Services.UserManagement;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Catbert4.Tests.Controllers.UserManagementControllerTests
{
    public partial class UserManagementControllerTests
    {
        #region GetVisibleByUser Tests

        [TestMethod]
        public void TestRoleServiceGetVisibleByUserReturnsExpectedResults1()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakeRoles(10, RoleRepository);
            var permissions = new List<Permission>();
            for (int i = 0; i < 7; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i+1));
                permissions[i].Application = ApplicationRepository.Queryable.First();
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            permissions[1].Application = ApplicationRepository.GetNullableById(2);
            permissions[2].User = UserRepository.GetNullableById(1);
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);

            applicationRoleRepository.Expect(a => a.Queryable).Return(new List<ApplicationRole>().AsQueryable()).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = RoleService.GetVisibleByUser("Name1", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Role>));
            Assert.AreEqual(5, result.Count());
            Assert.AreEqual("Name1", result.ElementAt(0).Name);
            Assert.AreEqual("Name4", result.ElementAt(1).Name);
            Assert.AreEqual("Name5", result.ElementAt(2).Name);
            #endregion Assert		
        }

        [TestMethod]
        public void TestRoleServiceGetVisibleByUserReturnsExpectedResults2()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakeRoles(10, RoleRepository);
            var permissions = new List<Permission>();
            for (int i = 0; i < 7; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i + 1));
                permissions[i].Application = ApplicationRepository.Queryable.First();
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            permissions[1].Application = ApplicationRepository.GetNullableById(2);
            permissions[2].User = UserRepository.GetNullableById(1);
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);

            applicationRoleRepository.Expect(a => a.Queryable).Return(new List<ApplicationRole>().AsQueryable()).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = RoleService.GetVisibleByUser("Name1", "LoginId3");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Role>));
            Assert.AreEqual(0, result.Count());

            #endregion Assert
        }

        [TestMethod]
        public void TestRoleServiceGetVisibleByUserReturnsExpectedResults3()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakeRoles(10, RoleRepository);
            var permissions = new List<Permission>();
            for (int i = 0; i < 7; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i + 1));
                permissions[i].Application = ApplicationRepository.Queryable.First();
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            permissions[1].Application = ApplicationRepository.GetNullableById(2);
            permissions[2].User = UserRepository.GetNullableById(1);
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);

            applicationRoleRepository.Expect(a => a.Queryable).Return(new List<ApplicationRole>().AsQueryable()).Repeat.Any();

            #endregion Arrange

            #region Act
            var result = RoleService.GetVisibleByUser("Name2", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Role>));
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Name2", result.ElementAt(0).Name);
            #endregion Assert
        }

        [TestMethod]
        public void TestRoleServiceGetVisibleByUserReturnsExpectedResults4()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);
            ControllerRecordFakes.FakeRoles(10, RoleRepository);
            var permissions = new List<Permission>();
            for (int i = 0; i < 10; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i + 1));
                permissions[i].Application = ApplicationRepository.Queryable.First();
                permissions[i].User = UserRepository.GetNullableById(3);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            permissions[4].User = UserRepository.GetNullableById(2);
            ControllerRecordFakes.FakePermissions(0, PermissionRepository, permissions);

            var applicationRoles = new List<ApplicationRole>();
            for (int i = 0; i < 10; i++)
            {
                applicationRoles.Add(CreateValidEntities.ApplicationRole(i+1));
                applicationRoles[i].Role = RoleRepository.GetNullableById(i + 1);
                applicationRoles[i].Application = ApplicationRepository.GetNullableById(1);
            }
            ControllerRecordFakes.FakeApplicationRoles(0, applicationRoleRepository, applicationRoles);

            #endregion Arrange

            #region Act
            //var manageableRoles = from ar in applicationRoleRepository.Queryable
            //                      where ar.Application.Name == "Name1" &&
            //                            ar.Level > (
            //                                           (from p in PermissionRepository.Queryable
            //                                            join a in applicationRoleRepository.Queryable on
            //                                                new { Role = p.Role.Id, App = p.Application.Id }
            //                                                equals new { Role = a.Role.Id, App = a.Application.Id }
            //                                            where p.Application.Name ==  "Name1" &&
            //                                                  p.User.LoginId == "LoginId2" &&
            //                                                  a.Level != null
            //                                            select a.Level).Max()
            //                                       )
            //                      select ar.Role;


            var result = RoleService.GetVisibleByUser("Name1", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Role>));
            Assert.AreEqual(6, result.Count());
            for (int i = 1; i < 6; i++)
            {
                Assert.AreEqual(string.Format("Name{0}", (i+4)), result.ElementAt(i).Name);
            }

            #endregion Assert
        }

        #endregion GetVisibleByUser Tests

        #region GetManagementRolesForUserInApplication Tests

        [TestMethod]
        public void TestGetManagementRolesForUserInApplicationReturnsExpectedValue1()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);

            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles.Add(CreateValidEntities.Role(2));
            roles.Add(CreateValidEntities.Role(3));
            roles.Add(CreateValidEntities.Role(4));
            roles[0].Name = "ManageAll";
            roles[1].Name = "ManageSchool";
            roles[2].Name = "ManageUnit";
            roles[3].Name = "ManageMe"; //Not special
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);

            var permissions = new List<Permission>();
            for (int i = 0; i < 6; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i+1));
                permissions[i].Application = ApplicationRepository.GetNullableById(1);
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakePermissions(6, PermissionRepository, permissions);
            #endregion Arrange

            #region Act
            var result = RoleService.GetManagementRolesForUserInApplication("Name1", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("ManageAll", result.ElementAt(0));
            Assert.AreEqual("ManageSchool", result.ElementAt(1));
            Assert.AreEqual("ManageUnit", result.ElementAt(2));
            Assert.AreEqual("ManageMe", result.ElementAt(3));
            #endregion Assert		
        }

        [TestMethod]
        public void TestGetManagementRolesForUserInApplicationReturnsExpectedValue2()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);

            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles.Add(CreateValidEntities.Role(2));
            roles.Add(CreateValidEntities.Role(3));
            roles.Add(CreateValidEntities.Role(4));
            roles[0].Name = "ManageAll";
            roles[1].Name = "ManageSchool";
            roles[2].Name = "ManageUnit";
            roles[3].Name = "ManageMe"; //Not special
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);

            var permissions = new List<Permission>();
            for (int i = 0; i < 6; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i + 1));
                permissions[i].Application = ApplicationRepository.GetNullableById(1);
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakePermissions(6, PermissionRepository, permissions);
            #endregion Arrange

            #region Act
            var result = RoleService.GetManagementRolesForUserInApplication("Name1", "LoginId3");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestGetManagementRolesForUserInApplicationReturnsExpectedValue3()
        {
            #region Arrange
            var applicationRoleRepository = FakeRepository<ApplicationRole>();
            RoleService = new RoleService(PermissionRepository, applicationRoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(3, UserRepository);

            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles.Add(CreateValidEntities.Role(2));
            roles.Add(CreateValidEntities.Role(3));
            roles.Add(CreateValidEntities.Role(4));
            roles[0].Name = "ManageAll";
            roles[1].Name = "ManageSchool";
            roles[2].Name = "ManageUnit";
            roles[3].Name = "ManageMe"; //Not special
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);

            var permissions = new List<Permission>();
            for (int i = 0; i < 6; i++)
            {
                permissions.Add(CreateValidEntities.Permission(i + 1));
                permissions[i].Application = ApplicationRepository.GetNullableById(1);
                permissions[i].User = UserRepository.GetNullableById(2);
                permissions[i].Role = RoleRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakePermissions(6, PermissionRepository, permissions);
            #endregion Arrange

            #region Act
            var result = RoleService.GetManagementRolesForUserInApplication("NameXX", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            #endregion Assert
        }


        #endregion GetManagementRolesForUserInApplication Tests
    }
}
