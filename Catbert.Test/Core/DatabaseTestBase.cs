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
                LoadApplications();
                LoadUnits();

                ts.CommitTransaction();
            }
        }

        private static void LoadUnits()
        {
            //Load Schools
            var school1 = new School {ShortDescription = "School1", LongDescription = "School1", Abbreviation = "School1"};
            school1.SetID("01");

            var school2 = new School { ShortDescription = "School2", LongDescription = "School2", Abbreviation = "School2" };
            school2.SetID("02");

            SchoolBLL.EnsurePersistent(school1, true);
            SchoolBLL.EnsurePersistent(school2, true);

            for (int i = 0; i < 3; i++)
            {
                var unit = new Unit();
                
                if (i==0)
                {
                    unit.FISCode = "AANS";
                    unit.School = school1;
                }
                else if (i == 1)
                {
                    unit.FISCode = "APLS";
                    unit.School = school1;
                }
                else if (i == 2)
                {
                    unit.FISCode = "CHEM";
                    unit.School = school2;
                }
                
                unit.ShortName = "School" + unit.FISCode;
                unit.FullName = unit.ShortName;
                
                UnitBLL.EnsurePersistent(unit);
            }
        }

        private static void LoadApplications()
        {
            for (int i = 0; i < 3; i++)
            {
                var app = new Application {Name = "App" + i};

                ApplicationBLL.EnsurePersistent(app);
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
