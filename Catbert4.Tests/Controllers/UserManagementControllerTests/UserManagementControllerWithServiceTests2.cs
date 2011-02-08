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
        #region CanUserManageGivenLogin Tests

        [TestMethod]
        public void TestCanUserManageGivenLogin1()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(6, UserRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();

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
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");

            //var unitsForLoginToManage = from u in UnitAssociationRepository.Queryable
            //                            where u.Application.Name == "Name1"
            //                                  && u.User.LoginId == "LoginId2"
            //                            select u.Unit;
            //var numIntersectingUnits1 = unitsForLoginToManage.ToFuture();
            ////The linq provider can't handle Intersect() so we need to turn them into Enumerable first.
            ////Using to future does this and makes the queries more efficient by batching them
            //var numIntersectingUnits = unitsForLoginToManage.ToFuture().Intersect(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4).ToFuture()).Count();
            #endregion Act

            #region Assert
            Assert.IsTrue(result);
            #endregion Assert		
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin2()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(6, UserRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 5; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(2);
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsTrue(result);
            #endregion Assert
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin3()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(6, UserRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 5; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(3); //Different LoginId
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(i + 1);
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(result);
            #endregion Assert
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin4()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(6, UserRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 5; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(i+1); 
                unitAssociations[i].Application = ApplicationRepository.Queryable.First();
                unitAssociations[i].Unit = UnitRepository.GetNullableById(3); //Different unit (not 2 or 4)
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(result);
            #endregion Assert
        }

        [TestMethod]
        public void TestCanUserManageGivenLogin5()
        {
            #region Arrange
            UserService = new UserService(UnitService, UnitAssociationRepository, PermissionRepository);
            ControllerRecordFakes.FakeUnits(6, UnitRepository);
            ControllerRecordFakes.FakeRoles(6, RoleRepository);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUsers(6, UserRepository);

            //These are the units that the current user has if he has the correct access
            UnitService.Expect(a => a.GetVisibleByUser("Name1", "UserName")).Return(UnitRepository.Queryable.Where(a => a.Id == 2 || a.Id == 4)).Repeat.Any();

            var unitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 5; i++)
            {
                unitAssociations.Add(CreateValidEntities.UnitAssociation(i + 1));
                unitAssociations[i].User = UserRepository.GetNullableById(i + 1);
                unitAssociations[i].Application = ApplicationRepository.GetNullableById(2); //Different app
                unitAssociations[i].Unit = UnitRepository.GetNullableById(2); 
            }
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            #endregion Arrange

            #region Act
            var result = UserService.CanUserManageGivenLogin("Name1", "UserName", "LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(result);
            #endregion Assert
        }

        #endregion CanUserManageGivenLogin Tests

        #region GetVisibleByUser Tests

        /// <summary>
        /// Has Manage All
        /// </summary>
        [TestMethod]
        public void TestGetVisibleByUserReturnsExpectedResults1()
        {
            #region Arrange
            IRepository<School> schoolRepository = FakeRepository<School>();
            Controller.Repository.Expect(a => a.OfType<School>()).Return(schoolRepository).Repeat.Any();
            UnitService = new UnitService(RoleService, schoolRepository, UnitRepository, UnitAssociationRepository);
            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles[0].Name = "ManageAll";
            roles.Add(CreateValidEntities.Role(2));
            roles[1].Name = "ManageSchool";
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            ControllerRecordFakes.FakeUnits(3, UnitRepository);
            RoleService.Expect(a => a.GetManagementRolesForUserInApplication("Name2", "UserName")).Return(RoleRepository.Queryable.Select(a => a.Name).ToList()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = UnitService.GetVisibleByUser("Name2", "UserName");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Unit>));
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("ShortName2", result.ElementAt(1).ShortName);
            #endregion Assert		
        }

        /// <summary>
        /// Has Manage School
        /// </summary>
        [TestMethod]
        public void TestGetVisibleByUserReturnsExpectedResults2()
        {
            #region Arrange
            var schoolRepository = FakeRepository<School>();
            Controller.Repository.Expect(a => a.OfType<School>()).Return(schoolRepository).Repeat.Any();
            UnitService = new UnitService(RoleService, schoolRepository, UnitRepository, UnitAssociationRepository);
            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles[0].Name = "ManageSchool";
            roles.Add(CreateValidEntities.Role(2));
            roles[1].Name = "ManageUnit";
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);
            
            var schools = new List<School>();
            schools.Add(CreateValidEntities.School(1));
            schools[0].Units = new List<Unit>();
            schools[0].Units.Add(CreateValidEntities.Unit(1));
            schools[0].Units[0].SetIdTo(1);
            ControllerRecordFakes.FakeSchools(3, schoolRepository, schools);

            var units = new List<Unit>();
            units.Add(CreateValidEntities.Unit(1));
            units.Add(CreateValidEntities.Unit(2));
            units.Add(CreateValidEntities.Unit(3));
            units[0].School = schoolRepository.Queryable.First();
            units[1].School = schoolRepository.Queryable.First();
            units[2].School = schoolRepository.Queryable.First();

            var unitAssociations = new List<UnitAssociation>();
            unitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            unitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            unitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            unitAssociations[0].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[0].Unit = units[0];
            unitAssociations[0].Unit.School = CreateValidEntities.School(1);
            unitAssociations[0].Unit.School.SetId("1");
            unitAssociations[0].User = CreateValidEntities.User(1);
            unitAssociations[0].User.LoginId = "UserName";

            unitAssociations[1].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[1].Unit = units[1];
            unitAssociations[1].Unit.School = CreateValidEntities.School(1);
            unitAssociations[1].Unit.School.SetId("1");
            unitAssociations[1].User = CreateValidEntities.User(1);
            unitAssociations[1].User.LoginId = "UserName";

            unitAssociations[2].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[2].Unit = units[2];
            unitAssociations[2].Unit.School = CreateValidEntities.School(1);
            unitAssociations[2].Unit.School.SetId("1");
            unitAssociations[2].User = CreateValidEntities.User(1);
            unitAssociations[2].User.LoginId = "UserName";

            units[0].UnitAssociations = new List<UnitAssociation>();
            units[1].UnitAssociations = new List<UnitAssociation>();
            units[2].UnitAssociations = new List<UnitAssociation>();
            units[0].UnitAssociations.Add(unitAssociations[0]);
            units[1].UnitAssociations.Add(unitAssociations[1]);
            units[1].UnitAssociations.Add(unitAssociations[2]);

            ControllerRecordFakes.FakeUnits(2, UnitRepository, units);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            RoleService.Expect(a => a.GetManagementRolesForUserInApplication("Name2", "UserName")).Return(RoleRepository.Queryable.Select(a => a.Name).ToList()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = UnitService.GetVisibleByUser("Name2", "UserName");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Unit>));
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ShortName1", result.ElementAt(0).ShortName);
            #endregion Assert
        }

        /// <summary>
        /// Has Manage School
        /// </summary>
        [TestMethod]
        public void TestGetVisibleByUserReturnsExpectedResults3()
        {
            #region Arrange
            var schoolRepository = FakeRepository<School>();
            Controller.Repository.Expect(a => a.OfType<School>()).Return(schoolRepository).Repeat.Any();
            UnitService = new UnitService(RoleService, schoolRepository, UnitRepository, UnitAssociationRepository);
            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles[0].Name = "ManageSchool";
            roles.Add(CreateValidEntities.Role(2));
            roles[1].Name = "ManageUnit";
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            var schools = new List<School>();
            schools.Add(CreateValidEntities.School(1));
            schools[0].Units = new List<Unit>();
            schools[0].Units.Add(CreateValidEntities.Unit(1));
            schools[0].Units[0].SetIdTo(1);
            schools[0].Units.Add(CreateValidEntities.Unit(2));
            schools[0].Units[0].SetIdTo(2);
            ControllerRecordFakes.FakeSchools(3, schoolRepository, schools);

            var units = new List<Unit>();
            units.Add(CreateValidEntities.Unit(1));
            units.Add(CreateValidEntities.Unit(2));
            units.Add(CreateValidEntities.Unit(3));
            units[0].School = schoolRepository.Queryable.First();
            units[1].School = schoolRepository.GetNullableById(2);
            units[2].School = schoolRepository.Queryable.First();

            var unitAssociations = new List<UnitAssociation>();
            unitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            unitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            unitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            unitAssociations[0].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[0].Unit = units[0];
            unitAssociations[0].Unit.School = CreateValidEntities.School(1);
            unitAssociations[0].Unit.School.SetId("1");
            unitAssociations[0].User = CreateValidEntities.User(1);
            unitAssociations[0].User.LoginId = "UserName";

            unitAssociations[1].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[1].Unit = units[1];
            unitAssociations[1].Unit.School = CreateValidEntities.School(1);
            unitAssociations[1].Unit.School.SetId("1");
            unitAssociations[1].User = CreateValidEntities.User(1);
            unitAssociations[1].User.LoginId = "UserName";

            unitAssociations[2].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[2].Unit = units[2];
            unitAssociations[2].Unit.School = CreateValidEntities.School(1);
            unitAssociations[2].Unit.School.SetId("1");
            unitAssociations[2].User = CreateValidEntities.User(1);
            unitAssociations[2].User.LoginId = "UserName";

            units[0].UnitAssociations = new List<UnitAssociation>();
            units[1].UnitAssociations = new List<UnitAssociation>();
            units[2].UnitAssociations = new List<UnitAssociation>();
            units[0].UnitAssociations.Add(unitAssociations[0]);
            units[1].UnitAssociations.Add(unitAssociations[1]);
            units[1].UnitAssociations.Add(unitAssociations[2]);

            ControllerRecordFakes.FakeUnits(2, UnitRepository, units);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            RoleService.Expect(a => a.GetManagementRolesForUserInApplication("Name2", "UserName")).Return(RoleRepository.Queryable.Select(a => a.Name).ToList()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = UnitService.GetVisibleByUser("Name2", "UserName");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Unit>));
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("ShortName2", result.ElementAt(1).ShortName);
            #endregion Assert
        }

        /// <summary>
        /// Has  manage unit
        /// </summary>
        [TestMethod]
        public void TestGetVisibleByUserReturnsExpectedResults4()
        {
            #region Arrange
            var schoolRepository = FakeRepository<School>();
            Controller.Repository.Expect(a => a.OfType<School>()).Return(schoolRepository).Repeat.Any();
            UnitService = new UnitService(RoleService, schoolRepository, UnitRepository, UnitAssociationRepository);
            var roles = new List<Role>();
            roles.Add(CreateValidEntities.Role(1));
            roles[0].Name = "ManageNot";
            roles.Add(CreateValidEntities.Role(2));
            roles[1].Name = "ManageUnit";
            ControllerRecordFakes.FakeRoles(5, RoleRepository, roles);
            ControllerRecordFakes.FakeApplications(3, ApplicationRepository);

            var schools = new List<School>();
            schools.Add(CreateValidEntities.School(1));
            schools[0].Units = new List<Unit>();
            schools[0].Units.Add(CreateValidEntities.Unit(1));
            schools[0].Units[0].SetIdTo(1);
            schools[0].Units.Add(CreateValidEntities.Unit(2));
            schools[0].Units[0].SetIdTo(2);
            ControllerRecordFakes.FakeSchools(3, schoolRepository, schools);

            var units = new List<Unit>();
            units.Add(CreateValidEntities.Unit(1));
            units.Add(CreateValidEntities.Unit(2));
            units.Add(CreateValidEntities.Unit(3));
            units[0].School = schoolRepository.Queryable.First();
            units[1].School = schoolRepository.GetNullableById(2);
            units[2].School = schoolRepository.Queryable.First();

            var unitAssociations = new List<UnitAssociation>();
            unitAssociations.Add(CreateValidEntities.UnitAssociation(1));
            unitAssociations.Add(CreateValidEntities.UnitAssociation(2));
            unitAssociations.Add(CreateValidEntities.UnitAssociation(3));
            unitAssociations[0].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[0].Unit = units[0];
            unitAssociations[0].Unit.School = CreateValidEntities.School(1);
            unitAssociations[0].Unit.School.SetId("1");
            unitAssociations[0].User = CreateValidEntities.User(1);
            unitAssociations[0].User.LoginId = "UserName";

            unitAssociations[1].Application = ApplicationRepository.GetNullableById(1);
            unitAssociations[1].Unit = units[1];
            unitAssociations[1].Unit.School = CreateValidEntities.School(1);
            unitAssociations[1].Unit.School.SetId("1");
            unitAssociations[1].User = CreateValidEntities.User(1);
            unitAssociations[1].User.LoginId = "UserName";

            unitAssociations[2].Application = ApplicationRepository.GetNullableById(2);
            unitAssociations[2].Unit = units[2];
            unitAssociations[2].Unit.School = CreateValidEntities.School(1);
            unitAssociations[2].Unit.School.SetId("1");
            unitAssociations[2].User = CreateValidEntities.User(1);
            unitAssociations[2].User.LoginId = "UserNameNot";

            units[0].UnitAssociations = new List<UnitAssociation>();
            units[1].UnitAssociations = new List<UnitAssociation>();
            units[2].UnitAssociations = new List<UnitAssociation>();
            units[0].UnitAssociations.Add(unitAssociations[0]);
            units[1].UnitAssociations.Add(unitAssociations[1]);
            units[1].UnitAssociations.Add(unitAssociations[2]);

            ControllerRecordFakes.FakeUnits(2, UnitRepository, units);
            ControllerRecordFakes.FakeUnitAssociations(0, UnitAssociationRepository, unitAssociations);
            RoleService.Expect(a => a.GetManagementRolesForUserInApplication("Name2", "UserName")).Return(RoleRepository.Queryable.Select(a => a.Name).ToList()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = UnitService.GetVisibleByUser("Name2", "UserName");
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IQueryable<Unit>));
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ShortName1", result.ElementAt(0).ShortName);
            #endregion Assert
        }


        [TestMethod]
        [ExpectedException(typeof(System.Security.Authentication.AuthenticationException))]
        public void TestGetVisibleByUserReturnsExpectedResults5()
        {
            try
            {
                #region Arrange
                IRepository<School> schoolRepository = FakeRepository<School>();
                Controller.Repository.Expect(a => a.OfType<School>()).Return(schoolRepository).Repeat.Any();
                UnitService = new UnitService(RoleService, schoolRepository, UnitRepository, UnitAssociationRepository);

                RoleService.Expect(a => a.GetManagementRolesForUserInApplication("Name2", "UserName")).Return(new List<string>()).Repeat.Any();
                #endregion Arrange

                #region Act
                UnitService.GetVisibleByUser("Name2", "UserName");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("User UserName does not have access to this application", ex.Message);
                #endregion Assert 
                throw;
            }



        }
        
        #endregion GetVisibleByUser Tests
    }
}
 