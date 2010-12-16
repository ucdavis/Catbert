using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Catbert4.Core.Domain;
using Catbert4.Core.Mappings;
using Catbert4.Tests.Core;
using Catbert4.Tests.Core.Extensions;
using Catbert4.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentNHibernate.Testing;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;
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
 
        #region Email Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Email with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithTooLongValueDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.Email = "x".RepeatTimes((50 + 1));
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
                Assert.AreEqual(50 + 1, user.Email.Length);
                var results = user.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: length must be between 0 and 50", "Email: not a well-formed email address");
                Assert.IsTrue(user.IsTransient());
                Assert.IsFalse(user.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Email with null value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithNullValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.Email = null;
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
        /// Tests the Email with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithEmptyStringSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.Email = string.Empty;
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
        /// Tests the Email with one character saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithMinimumLengthSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.Email = "x@t.t";
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
        /// Tests the Email with long value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithLongValueSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.Email = "x".RepeatTimes(44) + "@t.com";            
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, user.Email.Length);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #endregion Email Tests

        #region Phone Number Tests
        //Note, I hate regex, so I'm only going to provide a few emails that should be valid
        [TestMethod]
        public void TestUserWithValidPhoneNumbers()
        {
            var phones = new List<string>();
            phones.Add("555-555-5555");
            phones.Add("(555) 555-5555");
            phones.Add("555 555 5555");
            phones.Add("(555)555 5555");
            phones.Add("(555)555-5555");
            phones.Add("(555) 555 5555");
            phones.Add("5555555555");
            phones.Add(null);
            //phones.Add("5".RepeatTimes(51));
            //phones.Add(string.Empty);
            //phones.Add("Happy");

            foreach (var phone in phones)
            {
                TestPhone(phone);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUserWithInvalidPhoneNumbers()
        {
            var phones = new List<string>();
            //phones.Add("555-555-5555");
            //phones.Add("(555) 555-5555");
            //phones.Add("555 555 5555");
            //phones.Add("(555)555 5555");
            //phones.Add("(555)555-5555");
            //phones.Add("(555) 555 5555");
            //phones.Add("5555555555");
            //phones.Add(null);
            phones.Add("5".RepeatTimes(51));
            phones.Add(string.Empty);
            phones.Add("Happy");

            foreach (var phone in phones)
            {
                TestPhone(phone);
            }
        }

        private void TestPhone(string phone)
        {
            #region Arrange
            var user = GetValid(9);
            user.Phone = phone;
            #endregion Arrange

            try
            {
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
            catch (Exception ex)
            {
                throw new ApplicationException("Phone: " + phone + "\n" + ex.Message);
            }

        }
        #endregion Phone Number Tests

        #region Inactive Tests

        /// <summary>
        /// Tests the Inactive is false saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsFalseSaves()
        {
            #region Arrange

            User user = GetValid(9);
            user.Inactive = false;

            #endregion Arrange

            #region Act

            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(user.Inactive);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Inactive is true saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsTrueSaves()
        {
            #region Arrange

            var user = GetValid(9);
            user.Inactive = true;

            #endregion Arrange

            #region Act

            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(user.Inactive);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());

            #endregion Assert
        }

        #endregion Inactive Tests

        #region UnitAssociations Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestUserWithpopulatedNewUnitAssociationsDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.UnitAssociations = new List<UnitAssociation>();
                user.UnitAssociations.Add(CreateValidEntities.UnitAssociation(7));
                user.UnitAssociations.Add(CreateValidEntities.UnitAssociation(8));
                user.UnitAssociations.Add(CreateValidEntities.UnitAssociation(9));
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(user);
                Assert.AreEqual(3, user.UnitAssociations.Count);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.UnitAssociation, Entity: Catbert4.Core.Domain.UnitAssociation", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests
        #region Valid Tests
        [TestMethod]
        public void TestUserWithNullUnitAssociationsSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.UnitAssociations = null;
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(user.UnitAssociations);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestUserWithEmptyUnitAssociationsSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.UnitAssociations = new List<UnitAssociation>();
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(user.UnitAssociations);
            Assert.AreEqual(0, user.UnitAssociations.Count);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestUserWithExistingUnitAssociationsSaves()
        {
            #region Arrange
            var user = UserRepository.GetById(1);
            Repository.OfType<UnitAssociation>().DbContext.BeginTransaction();
            LoadUnitAssociations(3, user);
            Repository.OfType<UnitAssociation>().DbContext.CommitTransaction();            
   
            user.UnitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 3; i++)
            {
                user.UnitAssociations.Add(Repository.OfType<UnitAssociation>().GetById(i+1));
            }
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(user.UnitAssociations);
            Assert.AreEqual(3, user.UnitAssociations.Count);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }


        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod, Ignore] //This will not delete (because of the relations), but we never remove users anyway.
        public void TestUserWithExistingUnitAssociationsDoesNotCascadeDeleteToUnitAssociations()
        {
            #region Arrange
            var user = UserRepository.GetById(1);
            Repository.OfType<UnitAssociation>().DbContext.BeginTransaction();
            LoadUnitAssociations(3, user);
            Repository.OfType<UnitAssociation>().DbContext.CommitTransaction();
            var unitAssociationsCount = Repository.OfType<UnitAssociation>().Queryable.Count();
            Assert.IsTrue(unitAssociationsCount > 0);
            user.UnitAssociations = new List<UnitAssociation>();
            for (int i = 0; i < 3; i++)
            {
                user.UnitAssociations.Add(Repository.OfType<UnitAssociation>().GetById(i + 1));
            }
  
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();

            Assert.IsNotNull(user.UnitAssociations);
            Assert.AreEqual(3, user.UnitAssociations.Count);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());

            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.Remove(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(UserRepository.GetNullableById(1));
            Assert.AreEqual(unitAssociationsCount, Repository.OfType<UnitAssociation>().Queryable.Count());
            #endregion Assert
        }
        
        #endregion Cascade Tests
        #endregion UnitAssociations Tests

        #region Permissions Tests
        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestUserWithPopulatedNewPermissionsDoesNotSave()
        {
            User user = null;
            try
            {
                #region Arrange
                user = GetValid(9);
                user.Permissions = new List<Permission>();
                user.Permissions.Add(CreateValidEntities.Permission(7));
                user.Permissions.Add(CreateValidEntities.Permission(8));
                user.Permissions.Add(CreateValidEntities.Permission(9));
                #endregion Arrange

                #region Act
                UserRepository.DbContext.BeginTransaction();
                UserRepository.EnsurePersistent(user);
                UserRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(user);
                Assert.AreEqual(3, user.Permissions.Count);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.Permission, Entity: Catbert4.Core.Domain.Permission", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests
        [TestMethod]
        public void TestUserWithNullPermissionsSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.Permissions = null;
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(user.Permissions);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestUserWithEmptyPermissionsSaves()
        {
            #region Arrange
            var user = GetValid(9);
            user.Permissions = new List<Permission>();
            #endregion Arrange

            #region Act
            UserRepository.DbContext.BeginTransaction();
            UserRepository.EnsurePersistent(user);
            UserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(user.Permissions);
            Assert.AreEqual(0, user.Permissions.Count);
            Assert.IsFalse(user.IsTransient());
            Assert.IsTrue(user.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests

        #endregion Permissions Tests

        #region FullNameAndLogin Tests

        [TestMethod]
        public void TestFullNameAndLoginReturnsExpectedResult1()
        {
            #region Arrange
            var record = new User();
            record.FirstName = null;
            record.LastName = null;
            record.LoginId = "1234567890";
            #endregion Arrange

            #region Act
            var result = record.FullNameAndLogin;
            #endregion Act

            #region Assert
            Assert.AreEqual("  (1234567890)", result);
            #endregion Assert		
        }
        [TestMethod]
        public void TestFullNameAndLoginReturnsExpectedResult2()
        {
            #region Arrange
            var record = new User();
            record.FirstName = "Philip";
            record.LastName = "Fry";
            record.LoginId = "1234567890";
            #endregion Arrange

            #region Act
            var result = record.FullNameAndLogin;
            #endregion Act

            #region Assert
            Assert.AreEqual("Philip Fry (1234567890)", result);
            #endregion Assert
        }

        #endregion FullNameAndLogin Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapUser1()
        {
            #region Arrange
            var id = UserRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();      
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<User>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Email, "test@tests.com")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.Inactive, false)
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.LoginId, "LoginId")
                .CheckProperty(c => c.Phone, "555-555-5555")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void TestCanCorrectlyMapUser2()
        {
            #region Arrange
            var id = UserRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            try
            {
                #region Act/Assert
                new PersistenceSpecification<User>(session)
                    .CheckProperty(c => c.Id, id)
                    .CheckProperty(c => c.Email, "test@tests.com")
                    .CheckProperty(c => c.FirstName, "FirstName")
                    .CheckProperty(c => c.Inactive, true)
                    .CheckProperty(c => c.LastName, "LastName")
                    .CheckProperty(c => c.LoginId, "LoginId")
                    .CheckProperty(c => c.Phone, "555-555-5555")
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
        public void TestCanCorrectlyMapUser3()
        {
            #region Arrange
            var id = UserRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<User>(session, new UserEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Email, "test@tests.com")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.Inactive, false)
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.LoginId, "LoginId")
                .CheckProperty(c => c.Phone, "555-555-5555")
                .CheckProperty(c => c.Permissions, new List<Permission>())
                .CheckProperty(c => c.UnitAssociations, new List<UnitAssociation>())
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapUser4()
        {
            #region Arrange
            var id = UserRepository.Queryable.Max(x => x.Id) + 1;
            var user = new User();
            user.SetIdTo(id);
            var session = NHibernateSessionManager.Instance.GetSession();
            UserRepository.DbContext.BeginTransaction();
            LoadSchools(1);
            LoadUnits(1);
            LoadUnitAssociations(3, user);
            LoadApplications(1);
            UserRepository.DbContext.CommitTransaction();

            var unitAssociations = new List<UnitAssociation>();
            unitAssociations.Add(Repository.OfType<UnitAssociation>().GetById(1));
            unitAssociations.Add(Repository.OfType<UnitAssociation>().GetById(2));
            unitAssociations.Add(Repository.OfType<UnitAssociation>().GetById(3));

            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<User>(session, new UserEqualityComparer())
                .CheckProperty(c => c.Id, id)
                                .CheckProperty(c => c.Email, "test@tests.com")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.Inactive, false)
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.LoginId, "LoginId")
                .CheckProperty(c => c.Phone, "555-555-5555")
                .CheckProperty(c => c.Permissions, new List<Permission>())
                .CheckProperty(c => c.UnitAssociations, unitAssociations)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapUser5()
        {
            #region Arrange
            var id = UserRepository.Queryable.Max(x => x.Id) + 1;
            var user = new User();
            user.SetIdTo(id);
            var session = NHibernateSessionManager.Instance.GetSession();
            UserRepository.DbContext.BeginTransaction();
            LoadRoles(1);
            LoadApplications(1);
            LoadPermissions(3, user);
            UserRepository.DbContext.CommitTransaction();

            var permissions = new List<Permission>();
            permissions.Add(Repository.OfType<Permission>().GetById(1));
            permissions.Add(Repository.OfType<Permission>().GetById(2));
            permissions.Add(Repository.OfType<Permission>().GetById(3));

            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<User>(session, new UserEqualityComparer())
                .CheckProperty(c => c.Id, id)
                                .CheckProperty(c => c.Email, "test@tests.com")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.Inactive, false)
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.LoginId, "LoginId")
                .CheckProperty(c => c.Phone, "555-555-5555")
                .CheckProperty(c => c.Permissions, permissions)
                .CheckProperty(c => c.UnitAssociations, new List<UnitAssociation>())
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
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
            {
                "[NHibernate.Validator.Constraints.EmailAttribute()]", 
                "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"                 
            }));
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("FullNameAndLogin", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Inactive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("LoginId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)10)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Permissions", "System.Collections.Generic.IList`1[Catbert4.Core.Domain.Permission]", new List<string>()));
            expectedFields.Add(new NameAndType("Phone", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[NHibernate.Validator.Constraints.PatternAttribute(\"((\\+\\d{1,3}(-| )?\\(?\\d\\)?(-| )?\\d{1,5})|(\\(?\\d{2,6}\\)?))(-| )?(\\d{3,4})(-| )?(\\d{4})(( x| ext)\\d{1,5}){0,1}\", Message = \"The Phone Number format is not valid\")]"               
            }));
            expectedFields.Add(new NameAndType("UnitAssociations", "System.Collections.Generic.IList`1[Catbert4.Core.Domain.UnitAssociation]", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(User));

        }

        #endregion Reflection of Database.	
		
        public class UserEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is IList<UnitAssociation> && y is IList<UnitAssociation>)
                {
                    var xVal = (IList<UnitAssociation>)x;
                    var yVal = (IList<UnitAssociation>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
                    }
                    return true;
                }

                if (x is IList<Permission> && y is IList<Permission>)
                {
                    var xVal = (IList<Permission>)x;
                    var yVal = (IList<Permission>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
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