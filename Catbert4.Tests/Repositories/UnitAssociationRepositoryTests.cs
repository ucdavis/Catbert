using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Catbert4.Core.Domain;
using Catbert4.Core.Mappings;
using Catbert4.Tests.Core;
using Catbert4.Tests.Core.Extensions;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentNHibernate.Testing;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Catbert4.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		UnitAssociation
    /// LookupFieldName:	Id yrjuy
    /// </summary>
    [TestClass]
    public class UnitAssociationRepositoryTests : AbstractRepositoryTests<UnitAssociation, int, UnitAssociationMap>
    {
        /// <summary>
        /// Gets or sets the UnitAssociation repository.
        /// </summary>
        /// <value>The UnitAssociation repository.</value>
        public IRepository<UnitAssociation> UnitAssociationRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitAssociationRepositoryTests"/> class.
        /// </summary>
        public UnitAssociationRepositoryTests()
        {
            UnitAssociationRepository = new Repository<UnitAssociation>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override UnitAssociation GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.UnitAssociation(counter);
            rtValue.Unit = Repository.OfType<Unit>().GetById(1);
            rtValue.User = Repository.OfType<User>().GetById(1);
            rtValue.Application = Repository.OfType<Application>().GetById(1);

            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<UnitAssociation> GetQuery(int numberAtEnd)
        {
            return UnitAssociationRepository.Queryable.Where(a => a.Id == numberAtEnd);
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(UnitAssociation entity, int counter)
        {
            Assert.AreEqual(counter, entity.Id);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(UnitAssociation entity, ARTAction action)
        {
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(Repository.OfType<User>().GetNullableById(2).LoginId, entity.User.LoginId);
                    break;
                case ARTAction.Restore:
                    entity.User = UserRestoreValue;
                    break;
                case ARTAction.Update:
                    UserRestoreValue = entity.User;
                    entity.User = Repository.OfType<User>().GetNullableById(2);
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            UnitAssociationRepository.DbContext.BeginTransaction();
            LoadUsers(3);
            LoadApplications(3);
            LoadSchools(1);
            LoadUnits(3);
            LoadRecords(5);
            UnitAssociationRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Inactive Tests

        /// <summary>
        /// Tests the Inactive is false saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsFalseSaves()
        {
            #region Arrange

            UnitAssociation unitAssociation = GetValid(9);
            unitAssociation.Inactive = false;

            #endregion Arrange

            #region Act

            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.EnsurePersistent(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(unitAssociation.Inactive);
            Assert.IsFalse(unitAssociation.IsTransient());
            Assert.IsTrue(unitAssociation.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Inactive is true saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsTrueSaves()
        {
            #region Arrange

            var unitAssociation = GetValid(9);
            unitAssociation.Inactive = true;

            #endregion Arrange

            #region Act

            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.EnsurePersistent(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(unitAssociation.Inactive);
            Assert.IsFalse(unitAssociation.IsTransient());
            Assert.IsTrue(unitAssociation.IsValid());

            #endregion Assert
        }

        #endregion Inactive Tests

        #region User Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUnitAssociationWithNullUserDoesNotSave()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.User = null;
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unitAssociation);
                var results = unitAssociation.ValidationResults().AsMessageList();
                results.AssertErrorsAre("User: may not be null");
                Assert.IsTrue(unitAssociation.IsTransient());
                Assert.IsFalse(unitAssociation.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestUnitAssociationWithNewUserDoesNotSave()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.User = new User();
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unitAssociation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.UnitAssociation.User", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestUnitAssociationSavesWithAnExistingUser()
        {
            #region Arrange
            var unitAssociation = GetValid(9);
            var user = Repository.OfType<User>().GetById(2);
            unitAssociation.User = user;
            #endregion Arrange

            #region Act
            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.EnsurePersistent(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(user, unitAssociation.User);
            Assert.AreEqual("LoginId2", unitAssociation.User.LoginId);
            Assert.IsFalse(unitAssociation.IsTransient());
            Assert.IsTrue(unitAssociation.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestUnitAssociationWithUnsavedUserDoesNotSaveOrCascade()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.User = CreateValidEntities.User(99);
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unitAssociation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.UnitAssociation.User", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestUnitAssociationDeleteDoesNotCascadedToUser()
        {
            #region Arrange
            var userCount = Repository.OfType<User>().Queryable.Count();
            Assert.IsTrue(userCount > 0);
            var unitAssociationCount = UnitAssociationRepository.Queryable.Count();
            var unitAssociation = UnitAssociationRepository.GetById(2);
            #endregion Arrange

            #region Act
            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.Remove(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(userCount, Repository.OfType<User>().Queryable.Count());
            Assert.AreEqual(unitAssociationCount - 1, UnitAssociationRepository.Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion UnitAssociation Tests

        #region Application Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUnitAssociationWithNullApplicationDoesNotSave()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.Application = null;
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unitAssociation);
                var results = unitAssociation.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Application: may not be null");
                Assert.IsTrue(unitAssociation.IsTransient());
                Assert.IsFalse(unitAssociation.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestUnitAssociationWithNewApplicationDoesNotSave()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.Application = new Application();
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unitAssociation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.UnitAssociation.Application", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestUnitAssociationSavesWithAnExistingApplication()
        {
            #region Arrange
            var unitAssociation = GetValid(9);
            var application = Repository.OfType<Application>().GetById(2);
            unitAssociation.Application = application;
            #endregion Arrange

            #region Act
            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.EnsurePersistent(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(application, unitAssociation.Application);
            Assert.AreEqual("Name2", unitAssociation.Application.Name);
            Assert.IsFalse(unitAssociation.IsTransient());
            Assert.IsTrue(unitAssociation.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestUnitAssociationWithUnsavedApplicationDoesNotSaveOrCascade()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.Application = CreateValidEntities.Application(99);
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unitAssociation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.UnitAssociation.Application", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestUnitAssociationDeleteDoesNotCascadedToApplication()
        {
            #region Arrange
            var applicationCount = Repository.OfType<Application>().Queryable.Count();
            Assert.IsTrue(applicationCount > 0);
            var unitAssociationCount = UnitAssociationRepository.Queryable.Count();
            var unitAssociation = UnitAssociationRepository.GetById(2);
            #endregion Arrange

            #region Act
            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.Remove(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(applicationCount, Repository.OfType<Application>().Queryable.Count());
            Assert.AreEqual(unitAssociationCount - 1, UnitAssociationRepository.Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Application Tests

        #region Unit Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUnitAssociationWithNullUnitDoesNotSave()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.Unit = null;
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unitAssociation);
                var results = unitAssociation.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Unit: may not be null");
                Assert.IsTrue(unitAssociation.IsTransient());
                Assert.IsFalse(unitAssociation.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestUnitAssociationWithNewUnitDoesNotSave()
        {
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.Unit = new Unit();
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unitAssociation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.UnitAssociation.Unit", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestUnitAssociationSavesWithAnExistingUnit()
        {
            #region Arrange
            var unitAssociation = GetValid(9);
            var unit = Repository.OfType<Unit>().GetById(2);
            unitAssociation.Unit = unit;
            #endregion Arrange

            #region Act
            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.EnsurePersistent(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(unit, unitAssociation.Unit);
            Assert.AreEqual("FullName2", unitAssociation.Unit.FullName);
            Assert.IsFalse(unitAssociation.IsTransient());
            Assert.IsTrue(unitAssociation.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestUnitAssociationWithUnsavedUnitDoesNotSaveOrCascade()
        {
            var SchoolRepository = new RepositoryWithTypedId<School, string>();
            UnitAssociation unitAssociation = null;
            try
            {
                #region Arrange
                unitAssociation = GetValid(9);
                unitAssociation.Unit = CreateValidEntities.Unit(99);
                unitAssociation.Unit.School = SchoolRepository.GetById("1");
                #endregion Arrange

                #region Act
                UnitAssociationRepository.DbContext.BeginTransaction();
                UnitAssociationRepository.EnsurePersistent(unitAssociation);
                UnitAssociationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unitAssociation);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.UnitAssociation.Unit", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestUnitAssociationDeleteDoesNotCascadedToUnit()
        {
            #region Arrange
            var unitCount = Repository.OfType<Unit>().Queryable.Count();
            Assert.IsTrue(unitCount > 0);
            var unitAssociationCount = UnitAssociationRepository.Queryable.Count();
            var unitAssociation = UnitAssociationRepository.GetById(2);
            #endregion Arrange

            #region Act
            UnitAssociationRepository.DbContext.BeginTransaction();
            UnitAssociationRepository.Remove(unitAssociation);
            UnitAssociationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(unitCount, Repository.OfType<Application>().Queryable.Count());
            Assert.AreEqual(unitAssociationCount - 1, UnitAssociationRepository.Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Unit Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapUnitAssociation1()
        {
            #region Arrange
            var id = UnitAssociationRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var unit = Repository.OfType<Unit>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<UnitAssociation>(session, new UnitAssociationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.Unit, unit)
                .CheckProperty(c => c.User, user)
                .VerifyTheMappings();
            #endregion Act/Assert
        }
        [TestMethod]
        public void TestCanCorrectlyMapUnitAssociation2()
        {
            #region Arrange
            var id = UnitAssociationRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var unit = Repository.OfType<Unit>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<UnitAssociation>(session, new UnitAssociationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckReference(c => c.Application, application)
                .CheckReference(c => c.Unit, unit)
                .CheckReference(c => c.User, user)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapUnitAssociation3()
        {
            #region Arrange
            var id = UnitAssociationRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var unit = Repository.OfType<Unit>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<UnitAssociation>(session, new UnitAssociationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.Unit, unit)
                .CheckProperty(c => c.User, user)
                .CheckProperty(c => c.Inactive, false)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void TestCanCorrectlyMapUnitAssociation4()
        {           
            #region Arrange
            var id = UnitAssociationRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var unit = Repository.OfType<Unit>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            try
            {
                #region Act/Assert
                new PersistenceSpecification<UnitAssociation>(session, new UnitAssociationEqualityComparer())
                    .CheckProperty(c => c.Id, id)
                    .CheckProperty(c => c.Application, application)
                    .CheckProperty(c => c.Unit, unit)
                    .CheckProperty(c => c.User, user)
                    .CheckProperty(c => c.Inactive, true)
                    .VerifyTheMappings();
                #endregion Act/Assert
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Non-static method requires a target.", ex.Message);
                throw;
            }

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
            expectedFields.Add(new NameAndType("Application", "Catbert4.Core.Domain.Application", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Inactive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Unit", "Catbert4.Core.Domain.Unit", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("User", "Catbert4.Core.Domain.User", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(UnitAssociation));

        }

        #endregion Reflection of Database.	
		
        public class UnitAssociationEqualityComparer : IEqualityComparer
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


                if (x is Unit && y is Unit)
                {
                    var xVal = (Unit)x;
                    var yVal = (Unit)y;
                    Assert.AreEqual(xVal.FullName, yVal.FullName);
                    Assert.AreEqual(xVal.Id, yVal.Id);
                    return true;
                }

                if (x is User && y is User)
                {
                    var xVal = (User)x;
                    var yVal = (User)y;
                    Assert.AreEqual(xVal.LoginId, yVal.LoginId);
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