using System;
using CAESArch.Data.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.BLL;
using CAESArch.BLL;

namespace CAESDO.Catbert.Test.Core
{
    public class DatabaseTestBase
    {
        [TestInitialize]
        public void CreateDB()
        {
            Configuration config = new Configuration().Configure();
            //Create the DB using the schema export
            new NHibernate.Tool.hbm2ddl.SchemaExport(config).Execute(false, true, false, true, NHibernateSessionManager.Instance.GetSession().Connection, null);

            LoadData();
        }

        public virtual void LoadData()
        {
            using (var ts = new TransactionScope())
            {
                LoadUsers();

                ts.CommitTransaction();
            }
        }

        private static void LoadUsers()
        {
            for (int i = 0; i < 20; i++)
            {
                var user = new User {Email = TestHelper.TestEmail, FirstName = "First" + i, LastName = "Last" + i, LoginID = "login" + i};

                if (i < 5) user.Inactive = true;

                UserBLL.EnsurePersistent(user);
            }
        }
    }
}
