using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using Catbert4.Core.Mappings;
using Catbert4.Tests.Core;
using Catbert4.Tests.Core.Extensions;
using Catbert4.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Catbert4.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		ApplicationRole
    /// LookupFieldName:	Level
    /// </summary>
    [TestClass]
    public class ApplicationRoleRepositoryTests : AbstractRepositoryTests<ApplicationRole, int, ApplicationRoleMap>
    {
        /// <summary>
        /// Gets or sets the ApplicationRole repository.
        /// </summary>
        /// <value>The ApplicationRole repository.</value>
        public IRepository<ApplicationRole> ApplicationRoleRepository { get; set; }
        public IRepository<Application> ApplicationRepository { get; set; }
        public IRepository<Role> RoleRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleRepositoryTests"/> class.
        /// </summary>
        public ApplicationRoleRepositoryTests()
        {
            ApplicationRoleRepository = new Repository<ApplicationRole>();
            ApplicationRepository = new Repository<Application>();
            RoleRepository = new Repository<Role>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override ApplicationRole GetValid(int? counter)
        {
            return CreateValidEntities.ApplicationRole(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<ApplicationRole> GetQuery(int numberAtEnd)
        {
            return ApplicationRoleRepository.Queryable.Where(a => a.Level == numberAtEnd);
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(ApplicationRole entity, int counter)
        {
            Assert.AreEqual(counter, entity.Level);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(ApplicationRole entity, ARTAction action)
        {
            const int updateValue = 998;
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Level);
                    break;
                case ARTAction.Restore:
                    entity.Level = IntRestoreValue;
                    break;
                case ARTAction.Update:
                    IntRestoreValue = entity.Level;
                    entity.Level = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            ApplicationRoleRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            ApplicationRoleRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Application Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestApplicationRoleWithNewApplicationDoesNotSave()
        {
            ApplicationRole applicationRole = null;
            try
            {
                #region Arrange
                applicationRole = GetValid(9);
                applicationRole.Application = new Application();
                #endregion Arrange

                #region Act
                ApplicationRoleRepository.DbContext.BeginTransaction();
                ApplicationRoleRepository.EnsurePersistent(applicationRole);
                ApplicationRoleRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(applicationRole);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.Application, Entity: Catbert4.Core.Domain.Application", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests
        
        [TestMethod]
        public void TestApplicationRoleWithNullApplicationSaves()
        {
            #region Arrange
            var applicationRole = GetValid(9);
            applicationRole.Application = null;
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(applicationRole.Application);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestApplicationRoleWithExistingApplicationSaves()
        {
            #region Arrange
            ApplicationRepository.DbContext.BeginTransaction();
            base.LoadApplications(1);
            ApplicationRepository.DbContext.CommitTransaction();
            var applicationRole = GetValid(9);
            applicationRole.Application = ApplicationRepository.GetById(1);
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(applicationRole.Application);
            Assert.AreEqual("Name1", applicationRole.Application.Name);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestApplicationRoleDeleteDoesNotCascadeToApplication()
        {
            #region Arrange
            ApplicationRepository.DbContext.BeginTransaction();
            LoadApplications(3);
            ApplicationRepository.DbContext.CommitTransaction();
            var applicationRole = GetValid(9);
            applicationRole.Application = ApplicationRepository.GetById(2);

            var applicationRoleCount = ApplicationRoleRepository.Queryable.Count();

            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
 
            Assert.IsNotNull(applicationRole.Application);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            Assert.AreEqual(applicationRoleCount + 1, ApplicationRoleRepository.Queryable.Count());
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.Remove(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(applicationRoleCount, ApplicationRoleRepository.Queryable.Count());
            Assert.AreEqual(3, ApplicationRepository.Queryable.Count());
            #endregion Assert		
        }
        
        #endregion Cascade Tests

        #endregion Application Tests

        #region Roles Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestApplicationRoleWithNewRoleDoesNotSave()
        {
            ApplicationRole applicationRole = null;
            try
            {
                #region Arrange
                applicationRole = GetValid(9);
                applicationRole.Role = new Role();
                #endregion Arrange

                #region Act
                ApplicationRoleRepository.DbContext.BeginTransaction();
                ApplicationRoleRepository.EnsurePersistent(applicationRole);
                ApplicationRoleRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(applicationRole);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.Role, Entity: Catbert4.Core.Domain.Role", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestApplicationRoleWithNullRoleSaves()
        {
            #region Arrange
            var applicationRole = GetValid(9);
            applicationRole.Role = null;
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(applicationRole.Role);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestApplicationRoleWithExistingRoleSaves()
        {
            #region Arrange
            RoleRepository.DbContext.BeginTransaction();
            LoadRoles(1);
            RoleRepository.DbContext.CommitTransaction();
            var applicationRole = GetValid(9);
            applicationRole.Role = RoleRepository.GetById(1);
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(applicationRole.Role);
            Assert.AreEqual("Name1", applicationRole.Role.Name);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestApplicationRoleDeleteDoesNotCascadeToRole()
        {
            #region Arrange
            RoleRepository.DbContext.BeginTransaction();
            LoadRoles(3);
            RoleRepository.DbContext.CommitTransaction();
            var applicationRole = GetValid(9);
            applicationRole.Role = RoleRepository.GetById(2);

            var applicationRoleCount = ApplicationRoleRepository.Queryable.Count();

            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();

            Assert.IsNotNull(applicationRole.Role);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            Assert.AreEqual(applicationRoleCount + 1, ApplicationRoleRepository.Queryable.Count());
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.Remove(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(applicationRoleCount, ApplicationRoleRepository.Queryable.Count());
            Assert.AreEqual(3, RoleRepository.Queryable.Count());
            #endregion Assert
        }

        #endregion Cascade Tests

        #endregion Roles Tests

        #region Level Tests

        [TestMethod]
        public void TestApplicationRoleWithNullValueSaves()
        {
            #region Arrange
            var applicationRole = GetValid(9);
            applicationRole.Level = null;
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(applicationRole.Level);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestApplicationRoleWithZeroValueSaves()
        {
            #region Arrange
            var applicationRole = GetValid(9);
            applicationRole.Level = 0;
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, applicationRole.Level);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestApplicationRoleWithMaxValueSaves()
        {
            #region Arrange
            var applicationRole = GetValid(9);
            applicationRole.Level = int.MaxValue;
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, applicationRole.Level);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestApplicationRoleWithMinValueSaves()
        {
            #region Arrange
            var applicationRole = GetValid(9);
            applicationRole.Level = int.MinValue;
            #endregion Arrange

            #region Act
            ApplicationRoleRepository.DbContext.BeginTransaction();
            ApplicationRoleRepository.EnsurePersistent(applicationRole);
            ApplicationRoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, applicationRole.Level);
            Assert.IsFalse(applicationRole.IsTransient());
            Assert.IsTrue(applicationRole.IsValid());
            #endregion Assert
        }
        #endregion Level Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapApplicationRole1()
        {
            #region Arrange
            var id = ApplicationRoleRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<ApplicationRole>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Application, null)
                .CheckProperty(c => c.Role, null)
                .CheckProperty(c => c.Level, null)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapApplicationRole2()
        {
            #region Arrange
            var id = ApplicationRoleRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            ApplicationRepository.DbContext.BeginTransaction();
            LoadApplications(1);
            ApplicationRepository.DbContext.CommitTransaction();
            RoleRepository.DbContext.BeginTransaction();
            LoadRoles(1);
            RoleRepository.DbContext.CommitTransaction();
            var application = ApplicationRepository.GetById(1);
            var role = RoleRepository.GetById(1);
            Assert.IsNotNull(application);
            Assert.IsNotNull(role);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<ApplicationRole>(session, new ApplicationRoleEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.Role, role)
                .CheckProperty(c => c.Level, 7)
                .VerifyTheMappings();
            #endregion Act/Assert
        }


        #endregion Fluent Mapping Tests

        #region Reflection of Database.

        /// <summary>
        /// Tests all fields in the database have been tested.
        /// If this fails and no other tests, it means that a field has been added which has not been tested above.
        /// </summary>
        [TestMethod]
        public void TestAllFieldsInTheDatabaseHaveBeenTested()
        {
            #region Arrange
            var expectedFields = new List<NameAndType>();
            expectedFields.Add(new NameAndType("Application", "Catbert4.Core.Domain.Application", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Level", "System.Nullable`1[System.Int32]", new List<string>()));
            expectedFields.Add(new NameAndType("Role", "Catbert4.Core.Domain.Role", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(ApplicationRole));

        }

        #endregion Reflection of Database.	
		
        public class ApplicationRoleEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is Application && y is Application)
                {
                    var xVal = (Application)x;
                    var yVal = (Application)y;
                    Assert.AreEqual(xVal.Name, yVal.Name);
                    Assert.AreEqual(xVal.Id, yVal.Id);
                    return true;
                }


                if (x is Role && y is Role)
                {
                    var xVal = (Role)x;
                    var yVal = (Role)y;
                    Assert.AreEqual(xVal.Name, yVal.Name);
                    Assert.AreEqual(xVal.Id, yVal.Id);
                    return true;
                }
                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}