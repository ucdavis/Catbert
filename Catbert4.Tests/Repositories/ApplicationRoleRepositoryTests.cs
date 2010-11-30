using System;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using Catbert4.Core.Mappings;
using Catbert4.Tests.Core;
using Catbert4.Tests.Core.Extensions;
using Catbert4.Tests.Core.Helpers;
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
            base.LoadApplication(1);
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
        //Test does not create new application
        //Test does not delete application
        #endregion Cascade Tests

        #endregion Application Tests




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

            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));

            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(ApplicationRole));

        }

        #endregion Reflection of Database.	
		
		
    }
}