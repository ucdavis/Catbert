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
    }
}
