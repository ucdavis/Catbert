using System;
using CAESArch.BLL;
using CAESArch.Data.NHibernate;
using CAESDO.Catbert.BLL;
using CAESDO.Catbert.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;

namespace CAESDO.Catbert.Test.BLLTests
{
    [TestClass]
    public class UserTests
    {
        [TestInitialize]
        public void CreateDB()
        {
            Configuration config = new Configuration().Configure();
            //Create the DB using the schema export
            new NHibernate.Tool.hbm2ddl.SchemaExport(config).Execute(false, true, false, true, NHibernateSessionManager.Instance.GetSession().Connection, null);

            LoadData();
        }

        private static void LoadData()
        {
            LoadUsers();
            LoadRolesAndUnits();
            LoadApplications();
            LoadTracking();
        }

        [TestMethod]
        public void CanInsertNewUser()
        {
            var newUser = new User {FirstName = "Hermes", LastName = "Conrad", LoginID = "hconrad"};

            var result = UserBLL.InsertNewUserWithRoleAndUnit(newUser, "Role1", "Unit1", "App1", "pjfry");

            Assert.IsNotNull(result);
            Assert.AreEqual(newUser.LoginID, result.LoginID);
        }

        [TestMethod]
        public void CanGetUser()
        {
            var user = UserBLL.GetByID(1);

            Assert.IsNotNull(user);
            Assert.AreEqual("pjfry", user.LoginID);

        }

        private static void LoadUsers()
        {
            var u = new User {FirstName = "Philip", LastName = "Fry", LoginID = "pjfry"};

            using (var ts = new TransactionScope())
            {
                UserBLL.EnsurePersistent(u);

                ts.CommitTransaction();
            }
        }

        private static void LoadApplications()
        {
            var app = new Application { Name = "App1" };

            using (var ts = new TransactionScope())
            {
                ApplicationBLL.EnsurePersistent(app);
                ts.CommitTransaction();
            }
        }

        private static void LoadRolesAndUnits()
        {
            var role = new Role() { Name = "Role1" };
            var school = new School() { Abbreviation = "School", LongDescription = "School", ShortDescription = "School"};
            var unit = new Unit() { FISCode = "NEWW", FullName = "Unit1", ShortName = "Unit1", School = school };

            school.SetID("99");

            using (var ts = new TransactionScope())
            {
                RoleBLL.EnsurePersistent(role);
                SchoolBLL.EnsurePersistent(school, true);
                UnitBLL.EnsurePersistent(unit);

                ts.CommitTransaction();
            }
        }

        private static void LoadTracking()
        {
            var userType = new TrackingType() { Name = "User" };
            var permType = new TrackingType() { Name = "Permission" };
            var action = new TrackingAction() { Name = "Add" };

            using (var ts = new TransactionScope())
            {
                GenericBLL<TrackingType, int>.EnsurePersistent(userType);
                GenericBLL<TrackingType, int>.EnsurePersistent(permType);
                GenericBLL<TrackingAction, int>.EnsurePersistent(action);

                ts.CommitTransaction();
            }
        }
    }
}
