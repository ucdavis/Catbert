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
    /// Entity Name:		Role
    /// LookupFieldName:	Name yrjuy
    /// </summary>
    [TestClass]
    public class RoleRepositoryTests : AbstractRepositoryTests<Role, int, RoleMap>
    {
        /// <summary>
        /// Gets or sets the Role repository.
        /// </summary>
        /// <value>The Role repository.</value>
        public IRepository<Role> RoleRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepositoryTests"/> class.
        /// </summary>
        public RoleRepositoryTests()
        {
            RoleRepository = new Repository<Role>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Role GetValid(int? counter)
        {
            return CreateValidEntities.Role(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Role> GetQuery(int numberAtEnd)
        {
            return RoleRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Role entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Role entity, ARTAction action)
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
            RoleRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            RoleRepository.DbContext.CommitTransaction();
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
            Role role = null;
            try
            {
                #region Arrange
                role = GetValid(9);
                role.Name = null;
                #endregion Arrange

                #region Act
                RoleRepository.DbContext.BeginTransaction();
                RoleRepository.EnsurePersistent(role);
                RoleRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(role);
                var results = role.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(role.IsTransient());
                Assert.IsFalse(role.IsValid());
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
            Role role = null;
            try
            {
                #region Arrange
                role = GetValid(9);
                role.Name = string.Empty;
                #endregion Arrange

                #region Act
                RoleRepository.DbContext.BeginTransaction();
                RoleRepository.EnsurePersistent(role);
                RoleRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(role);
                var results = role.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(role.IsTransient());
                Assert.IsFalse(role.IsValid());
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
            Role role = null;
            try
            {
                #region Arrange
                role = GetValid(9);
                role.Name = " ";
                #endregion Arrange

                #region Act
                RoleRepository.DbContext.BeginTransaction();
                RoleRepository.EnsurePersistent(role);
                RoleRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(role);
                var results = role.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                Assert.IsTrue(role.IsTransient());
                Assert.IsFalse(role.IsValid());
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
            Role role = null;
            try
            {
                #region Arrange
                role = GetValid(9);
                role.Name = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                RoleRepository.DbContext.BeginTransaction();
                RoleRepository.EnsurePersistent(role);
                RoleRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(role);
                Assert.AreEqual(50 + 1, role.Name.Length);
                var results = role.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: length must be between 0 and 50");
                Assert.IsTrue(role.IsTransient());
                Assert.IsFalse(role.IsValid());
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
            var role = GetValid(9);
            role.Name = "x";
            #endregion Arrange

            #region Act
            RoleRepository.DbContext.BeginTransaction();
            RoleRepository.EnsurePersistent(role);
            RoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(role.IsTransient());
            Assert.IsTrue(role.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var role = GetValid(9);
            role.Name = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            RoleRepository.DbContext.BeginTransaction();
            RoleRepository.EnsurePersistent(role);
            RoleRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, role.Name.Length);
            Assert.IsFalse(role.IsTransient());
            Assert.IsTrue(role.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region Inactive Tests

        /// <summary>
        /// Tests the Inactive is false saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsFalseSaves()
        {
            #region Arrange

            Role role = GetValid(9);
            role.Inactive = false;

            #endregion Arrange

            #region Act

            RoleRepository.DbContext.BeginTransaction();
            RoleRepository.EnsurePersistent(role);
            RoleRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(role.Inactive);
            Assert.IsFalse(role.IsTransient());
            Assert.IsTrue(role.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Inactive is true saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsTrueSaves()
        {
            #region Arrange

            var role = GetValid(9);
            role.Inactive = true;

            #endregion Arrange

            #region Act

            RoleRepository.DbContext.BeginTransaction();
            RoleRepository.EnsurePersistent(role);
            RoleRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(role.Inactive);
            Assert.IsFalse(role.IsTransient());
            Assert.IsTrue(role.IsValid());

            #endregion Assert
        }

        #endregion Inactive Tests

        #region Fluent Mapping Tests
        /// <summary>
        /// The mapping has a where clause that hides inacive roles. So this trows an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void TestCanCorrectlyMapApplicationRole1()
        {
            #region Arrange
            var id = RoleRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            try
            {
                #region Act/Assert
                new PersistenceSpecification<Role>(session)
                    .CheckProperty(c => c.Id, id)
                    .CheckProperty(c => c.Name, "Name")
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

        [TestMethod]
        public void TestCanCorrectlyMapApplicationRole2()
        {
            #region Arrange
            var id = RoleRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Role>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Name, "Name")
                .CheckProperty(c => c.Inactive, false)
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

            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Inactive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Role));

        }

        #endregion Reflection of Database.	
		
		
    }
}