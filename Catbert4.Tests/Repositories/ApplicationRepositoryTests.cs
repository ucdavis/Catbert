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
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Catbert4.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		Application
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class ApplicationRepositoryTests : AbstractRepositoryTests<Application, int, ApplicationMap>
    {
        /// <summary>
        /// Gets or sets the Application repository.
        /// </summary>
        /// <value>The Application repository.</value>
        public IRepository<Application> ApplicationRepository { get; set; }
        public IRepository<Role> RoleRepository { get; set; }
        public IRepository<ApplicationRole> ApplicationRoleRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRepositoryTests"/> class.
        /// </summary>
        public ApplicationRepositoryTests()
        {
            ApplicationRepository = new Repository<Application>();
            RoleRepository = new Repository<Role>();
            ApplicationRoleRepository = new Repository<ApplicationRole>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Application GetValid(int? counter)
        {
            return CreateValidEntities.Application(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Application> GetQuery(int numberAtEnd)
        {
            return ApplicationRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Application entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Application entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Name);
                    break;
                case ARTAction.Restore:
                    entity.Name = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Name;
                    entity.Name = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            ApplicationRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            ApplicationRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Name Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Name with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithNullValueDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Name = null;
                #endregion Arrange

                #region Act
                ApplicationRepository.DbContext.BeginTransaction();
                ApplicationRepository.EnsurePersistent(application);
                ApplicationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(application);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithEmptyStringDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Name = string.Empty;
                #endregion Arrange

                #region Act
                ApplicationRepository.DbContext.BeginTransaction();
                ApplicationRepository.EnsurePersistent(application);
                ApplicationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(application);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithSpacesOnlyDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Name = " ";
                #endregion Arrange

                #region Act
                ApplicationRepository.DbContext.BeginTransaction();
                ApplicationRepository.EnsurePersistent(application);
                ApplicationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(application);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithTooLongValueDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Name = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                ApplicationRepository.DbContext.BeginTransaction();
                ApplicationRepository.EnsurePersistent(application);
                ApplicationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(application);
                Assert.AreEqual(50 + 1, application.Name.Length);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: length must be between 0 and 50");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Name = "x";
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Name = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, application.Name.Length);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests
  
        #region Abbr Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Abbr with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAbbrWithTooLongValueDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Abbr = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                ApplicationRepository.DbContext.BeginTransaction();
                ApplicationRepository.EnsurePersistent(application);
                ApplicationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(application);
                Assert.AreEqual(50 + 1, application.Abbr.Length);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Abbr: length must be between 0 and 50");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Abbr with null value saves.
        /// </summary>
        [TestMethod]
        public void TestAbbrWithNullValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Abbr = null;
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Abbr with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestAbbrWithEmptyStringSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Abbr = string.Empty;
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Abbr with one space saves.
        /// </summary>
        [TestMethod]
        public void TestAbbrWithOneSpaceSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Abbr = " ";
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Abbr with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAbbrWithOneCharacterSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Abbr = "x";
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Abbr with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAbbrWithLongValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Abbr = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, application.Abbr.Length);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Abbr Tests

        #region Location Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Location with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLocationWithTooLongValueDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Location = "x".RepeatTimes((256 + 1));
                #endregion Arrange

                #region Act
                ApplicationRepository.DbContext.BeginTransaction();
                ApplicationRepository.EnsurePersistent(application);
                ApplicationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(application);
                Assert.AreEqual(256 + 1, application.Location.Length);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Location: length must be between 0 and 256");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Location with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithNullValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Location = null;
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithEmptyStringSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Location = string.Empty;
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with one space saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithOneSpaceSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Location = " ";
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithOneCharacterSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Location = "x";
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithLongValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Location = "x".RepeatTimes(256);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(256, application.Location.Length);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Location Tests

        #region Inactive Tests

        /// <summary>
        /// Tests the Inactive is false saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsFalseSaves()
        {
            #region Arrange

            Application application = GetValid(9);
            application.Inactive = false;

            #endregion Arrange

            #region Act

            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(application.Inactive);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Inactive is true saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsTrueSaves()
        {
            #region Arrange

            var application = GetValid(9);
            application.Inactive = true;

            #endregion Arrange

            #region Act

            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(application.Inactive);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());

            #endregion Assert
        }

        #endregion Inactive Tests

        #region Application Roles Tests

        [TestMethod]
        public void TestApplicationWithNullApplicationRolesSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.ApplicationRoles = null;
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(application.ApplicationRoles);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestApplicationWithEmptyListApplicationRolesSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.ApplicationRoles = new List<ApplicationRole>();
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(application.ApplicationRoles);
            Assert.AreEqual(0, application.ApplicationRoles.Count);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }
        [TestMethod]
        public void TestApplicationWithOneApplicationRolesSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(1));
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(application.ApplicationRoles);
            Assert.AreEqual(1, application.ApplicationRoles.Count);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestApplicationWithPopulatedApplicationRolesSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(1));
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(2));
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(3));
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(application.ApplicationRoles);
            Assert.AreEqual(3, application.ApplicationRoles.Count);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestApplicationCascadesAddToApplicationRoles()
        {
            #region Arrange
            RoleRepository.DbContext.BeginTransaction();
            LoadRoles(3);
            RoleRepository.DbContext.CommitTransaction();
            var applicationRoleCount = ApplicationRoleRepository.Queryable.Count();
            var application = GetValid(9);                      
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(1));
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(2));
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(3));
            application.ApplicationRoles[0].Role = RoleRepository.GetById(1);
            application.ApplicationRoles[1].Role = RoleRepository.GetById(2);
            application.ApplicationRoles[2].Role = RoleRepository.GetById(3);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(application.ApplicationRoles);
            Assert.AreEqual(3, application.ApplicationRoles.Count);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            Assert.AreEqual(applicationRoleCount + 3, ApplicationRoleRepository.Queryable.Count());
            #endregion Assert
        }

        [TestMethod]
        public void TestApplicationCascadesDeleteToApplicationRoles()
        {
            #region Arrange
            RoleRepository.DbContext.BeginTransaction();
            LoadRoles(3);
            RoleRepository.DbContext.CommitTransaction();
            var applicationRoleCount = ApplicationRoleRepository.Queryable.Count();
            var application = GetValid(9);
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(1));
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(2));
            application.ApplicationRoles.Add(CreateValidEntities.ApplicationRole(3));
            application.ApplicationRoles[0].Role = RoleRepository.GetById(1);
            application.ApplicationRoles[1].Role = RoleRepository.GetById(2);
            application.ApplicationRoles[2].Role = RoleRepository.GetById(3);

            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();

            Assert.IsNotNull(application.ApplicationRoles);
            Assert.AreEqual(3, application.ApplicationRoles.Count);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            Assert.AreEqual(applicationRoleCount + 3, ApplicationRoleRepository.Queryable.Count());
            var saveId = application.Id;
            Assert.IsTrue(ApplicationRepository.Queryable.Where(a => a.Id == saveId).Any());
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.Remove(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(ApplicationRepository.Queryable.Where(a => a.Id == saveId).Any());
            Assert.AreEqual(applicationRoleCount, ApplicationRoleRepository.Queryable.Count());
            #endregion Assert
        }

        #endregion Application Roles Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapApplication1()
        {
            #region Arrange
            var id = ApplicationRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Application>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Abbr, "Abbr")
                .CheckProperty(c => c.Inactive, true)
                .CheckProperty(c => c.Location, "Location")
                .CheckProperty(c => c.Name, "Name")
                //.CheckProperty(c => c.ApplicationRoles, applicationRoles)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapApplication2()
        {
            #region Arrange
            var id = ApplicationRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Application>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Abbr, "Abbr")
                .CheckProperty(c => c.Inactive, false)
                .CheckProperty(c => c.Location, "Location")
                .CheckProperty(c => c.Name, "Name")
                //.CheckProperty(c => c.ApplicationRoles, applicationRoles)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapApplication3()
        {
            #region Arrange
            var id = ApplicationRepository.Queryable.Max(x => x.Id) + 1;
            var application = new Application();
            application.SetIdTo(id);
            var session = NHibernateSessionManager.Instance.GetSession();
            RoleRepository.DbContext.BeginTransaction();
            LoadRoles(3);
            RoleRepository.DbContext.CommitTransaction();
            var applicationRoles = new List<ApplicationRole>();
            applicationRoles.Add(CreateValidEntities.ApplicationRole(1));
            applicationRoles.Add(CreateValidEntities.ApplicationRole(2));
            applicationRoles.Add(CreateValidEntities.ApplicationRole(3));
            applicationRoles[0].Role = RoleRepository.GetById(1);            
            applicationRoles[1].Role = RoleRepository.GetById(2);
            applicationRoles[2].Role = RoleRepository.GetById(3);
            applicationRoles[0].Application = application;
            applicationRoles[1].Application = application;
            applicationRoles[2].Application = application;
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Application>(session, new ApplicationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.ApplicationRoles, applicationRoles)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        #endregion Fluent Mapping Tests

        #region Misc Tests

        [TestMethod]
        public void TestConstructorPopulatesExpectedValues()
        {
            #region Arrange
            var application = new Application();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsNotNull(application.ApplicationRoles);
            Assert.IsNull(application.Abbr);
            Assert.IsNull(application.Location);
            Assert.IsNull(application.Name);
            Assert.IsFalse(application.Inactive);
            #endregion Assert		
        }

        [TestMethod]
        public void TestAllValuesPopulatedSaves()
        {
            #region Arrange
            Application application = CreateValidEntities.Application(9, true);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(application.Inactive);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert	
        }

        #endregion Misc Tests
        
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
            expectedFields.Add(new NameAndType("Abbr", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("ApplicationRoles", "System.Collections.Generic.IList`1[Catbert4.Core.Domain.ApplicationRole]", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Inactive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Location", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)256)]",
                 "[System.ComponentModel.DataAnnotations.DataTypeAttribute((System.ComponentModel.DataAnnotations.DataType)12)]"
            }));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Application));

        }

        #endregion Reflection of Database.	
		
        public class ApplicationEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is IList<ApplicationRole> && y is IList<ApplicationRole>)
                {
                    var xVal = (IList<ApplicationRole>)x;
                    var yVal = (IList<ApplicationRole>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Level, yVal[i].Level);
                        Assert.AreEqual(xVal[i].Role.Name, yVal[i].Role.Name);
                    }
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