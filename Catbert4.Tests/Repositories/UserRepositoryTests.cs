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
    /// Entity Name:		User
    /// LookupFieldName:	LoginId yrjuy
    /// </summary>
    [TestClass]
    public class UserRepositoryTests : AbstractRepositoryTests<User, int, UserMap>
    {
        /// <summary>
        /// Gets or sets the User repository.
        /// </summary>
        /// <value>The User repository.</value>
        public IRepository<User> UserRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class.
        /// </summary>
        public UserRepositoryTests()
        {
            UserRepository = new Repository<User>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override User GetValid(int? counter)
        {
            return CreateValidEntities.User(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<User> GetQuery(int numberAtEnd)
        {
            return UserRepository.Queryable.Where(a => a.LoginId.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(User entity, int counter)
        {
            Assert.AreEqual("LoginId" + counter, entity.LoginId);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(User entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.LoginId);
                    break;
                case ARTAction.Restore:
                    entity.LoginId = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.LoginId;
                    entity.LoginId = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            UserRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            UserRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region FirstName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the FirstName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFirstNameWithTooLongValueDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.FirstName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(user);
                Assert.AreEqual(50 + 1, user.FirstName.Length);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FirstName: length must be between 0 and 50");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the FirstName with null value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithNullValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.FirstName = null;
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithEmptyStringSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.FirstName = string.Empty;
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with one space saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneSpaceSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.FirstName = " ";
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneCharacterSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.FirstName = "x";
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithLongValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.FirstName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, user.FirstName.Length);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion FirstName Tests

        #region LastName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the LastName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLastNameWithTooLongValueDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.LastName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(user);
                Assert.AreEqual(50 + 1, user.LastName.Length);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LastName: length must be between 0 and 50");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LastName with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithNullValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LastName = null;
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithEmptyStringSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LastName = string.Empty;
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with one space saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneSpaceSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LastName = " ";
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneCharacterSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LastName = "x";
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithLongValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LastName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, user.LastName.Length);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LastName Tests

        #region LoginId Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the LoginId with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginIdWithNullValueDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.LoginId = null;
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(user);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LoginId: may not be null or empty");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LoginId with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginIdWithEmptyStringDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.LoginId = string.Empty;
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(user);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LoginId: may not be null or empty");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LoginId with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginIdWithSpacesOnlyDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.LoginId = " ";
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(user);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LoginId: may not be null or empty");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LoginId with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginIdWithTooLongValueDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.LoginId = "x".RepeatTimes((10 + 1));
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(user);
                Assert.AreEqual(10 + 1, user.LoginId.Length);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LoginId: length must be between 0 and 10");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LoginId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithOneCharacterSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LoginId = "x";
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithLongValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.LoginId = "x".RepeatTimes(10);
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(10, user.LoginId.Length);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LoginId Tests
 
        
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
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("LoginId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)10)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(User));

        }

        #endregion Reflection of Database.	
		
		
    }
}