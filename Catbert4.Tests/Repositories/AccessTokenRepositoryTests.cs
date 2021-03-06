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
    /// Entity Name:		AccessToken
    /// LookupFieldName:	Reason yrjuy
    /// </summary>
    [TestClass]
    public class AccessTokenRepositoryTests : AbstractRepositoryTests<AccessToken, int, AccessTokenMap>
    {
        /// <summary>
        /// Gets or sets the AccessToken repository.
        /// </summary>
        /// <value>The AccessToken repository.</value>
        public IRepository<AccessToken> AccessTokenRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenRepositoryTests"/> class.
        /// </summary>
        public AccessTokenRepositoryTests()
        {
            AccessTokenRepository = new Repository<AccessToken>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override AccessToken GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.AccessToken(counter);
            rtValue.Application = Repository.OfType<Application>().Queryable.First();
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<AccessToken> GetQuery(int numberAtEnd)
        {
            return AccessTokenRepository.Queryable.Where(a => a.Reason.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(AccessToken entity, int counter)
        {
            Assert.AreEqual("Reason" + counter, entity.Reason);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(AccessToken entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Reason);
                    break;
                case ARTAction.Restore:
                    entity.Reason = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Reason;
                    entity.Reason = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            Repository.OfType<Application>().DbContext.BeginTransaction();
            LoadApplications(2);
            Repository.OfType<Application>().DbContext.CommitTransaction();
            AccessTokenRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            AccessTokenRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Token Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Token with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTokenWithNullValueDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Token = null;
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Token: may not be null or empty");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Token with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTokenWithEmptyStringDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Token = string.Empty;
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Token: may not be null or empty", "Token: Token must be 32 characters long");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Token with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTokenWithSpacesOnlyDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Token = " ";
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Token: may not be null or empty", "Token: Token must be 32 characters long");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTokenWith32SpacesOnlyDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Token = " ".RepeatTimes(32);
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Token: may not be null or empty");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Token with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTokenWithTooLongValueDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Token = "x".RepeatTimes((32 + 1));
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                Assert.AreEqual(32 + 1, accessToken.Token.Length);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Token: Token must be 32 characters long");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTokenWithTooShortValueDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Token = "x".RepeatTimes(31);
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                Assert.AreEqual(31, accessToken.Token.Length);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Token: Token must be 32 characters long");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests


        /// <summary>
        /// Tests the Token with long value saves.
        /// </summary>
        [TestMethod]
        public void TestTokenWithLongValueSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.Token = "x".RepeatTimes(32);
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(32, accessToken.Token.Length);
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Token Tests

        #region Reason Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Reason with null value saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithNullValueSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.Reason = null;
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Reason with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithEmptyStringSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.Reason = string.Empty;
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Reason with one space saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithOneSpaceSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.Reason = " ";
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Reason with one character saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithOneCharacterSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.Reason = "x";
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Reason with long value saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithLongValueSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.Reason = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, accessToken.Reason.Length);
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Reason Tests

        #region Active Tests

        /// <summary>
        /// Tests the Active is false saves.
        /// </summary>
        [TestMethod]
        public void TestActiveIsFalseSaves()
        {
            #region Arrange

            AccessToken accessToken = GetValid(9);
            accessToken.Active = false;

            #endregion Arrange

            #region Act

            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(accessToken.Active);
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Active is true saves.
        /// </summary>
        [TestMethod]
        public void TestActiveIsTrueSaves()
        {
            #region Arrange

            var accessToken = GetValid(9);
            accessToken.Active = true;

            #endregion Arrange

            #region Act

            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(accessToken.Active);
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());

            #endregion Assert
        }

        #endregion Active Tests

        #region ContactEmail Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ContactEmail with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestContactEmailWithNullValueDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.ContactEmail = null;
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ContactEmail: may not be null or empty");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ContactEmail with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestContactEmailWithEmptyStringDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.ContactEmail = string.Empty;
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ContactEmail: may not be null or empty");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ContactEmail with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestContactEmailWithSpacesOnlyDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.ContactEmail = " ";
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ContactEmail: may not be null or empty", "ContactEmail: not a well-formed email address");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ContactEmail with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestContactEmailWithInvalidEmailValueDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.ContactEmail = "@x.com";
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ContactEmail: not a well-formed email address");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ContactEmail with 4 characters saves.
        /// </summary>
        [TestMethod]
        public void TestContactEmailWith4CharactersSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.ContactEmail = "x@x.x";
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ContactEmail with long value saves.
        /// </summary>
        [TestMethod]
        public void TestContactEmailWithLongValueSaves()
        {
            #region Arrange
            var accessToken = GetValid(9);
            accessToken.ContactEmail = "x".RepeatTimes(1000) + "@x.com";
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ContactEmail Tests

        #region Application Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the Application with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestApplicationWithAValueOfNullDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Application = null;
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(accessToken);
                Assert.AreEqual(accessToken.Application, null);
                var results = accessToken.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Application: may not be null");
                Assert.IsTrue(accessToken.IsTransient());
                Assert.IsFalse(accessToken.IsValid());
                throw;
            }	
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestApplicationWithANewValueDoesNotSave()
        {
            AccessToken accessToken = null;
            try
            {
                #region Arrange
                accessToken = GetValid(9);
                accessToken.Application = CreateValidEntities.Application(7);
                #endregion Arrange

                #region Act
                AccessTokenRepository.DbContext.BeginTransaction();
                AccessTokenRepository.EnsurePersistent(accessToken);
                AccessTokenRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(accessToken);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.Application, Entity: Name7", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestApplicationWithExistingValueSaves()
        {
            #region Arrange

            var accessToken = GetValid(9);
            accessToken.Application = Repository.OfType<Application>().GetNullableById(2);
            Assert.IsNotNull(accessToken.Application);
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.EnsurePersistent(accessToken);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsTrue(accessToken.Active);
            Assert.IsFalse(accessToken.IsTransient());
            Assert.IsTrue(accessToken.IsValid());
            #endregion Assert		
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestDeleteAccessTokenDoesNotCascadeToApplication()
        {
            #region Arrange
            var record = AccessTokenRepository.GetNullableById(2);
            Assert.IsNotNull(record);
            Assert.IsNotNull(record.Application);
            var count = Repository.OfType<Application>().Queryable.Count();
            Assert.IsTrue(count > 0);
            #endregion Arrange

            #region Act
            AccessTokenRepository.DbContext.BeginTransaction();
            AccessTokenRepository.Remove(record);
            AccessTokenRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(count, Repository.OfType<Application>().Queryable.Count());
            Assert.IsNull(AccessTokenRepository.GetNullableById(2));
            #endregion Assert		
        }
        #endregion Cascade Tests

        #endregion Application Tests

        #region Constructor Tests

        [TestMethod]
        public void TestConstructorSetsExpectedValues()
        {
            #region Arrange
            var record = new AccessToken();
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.Active);
            Assert.IsNull(record.Application);
            Assert.IsNull(record.ContactEmail);
            Assert.IsNull(record.Reason);
            Assert.IsNull(record.Token);
            #endregion Assert		
        }
        #endregion Constructor Tests

        #region Fluent Mapping Tests

        [TestMethod]
        public void TestCanCorrectlyMapAccessToken1()
        {
            #region Arrange
            var id = AccessTokenRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetNullableById(1);
            var temp = new AccessToken();
            temp.SetNewToken();
            var token = temp.Token; 
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<AccessToken>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Active, true)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.ContactEmail, "Test@testy.com")
                .CheckProperty(c => c.Reason, "Reason")
                .CheckProperty(c => c.Token, token)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapAccessToken2()
        {
            #region Arrange
            var id = AccessTokenRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetNullableById(1);
            var temp = new AccessToken();
            temp.SetNewToken();
            var token = temp.Token;
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<AccessToken>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Active, false)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.ContactEmail, "Test@testy.com")
                .CheckProperty(c => c.Reason, "Reason")
                .CheckProperty(c => c.Token, token)
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
            expectedFields.Add(new NameAndType("Active", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Application", "Catbert4.Core.Domain.Application", new List<string>
            {
                 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ContactEmail", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.EmailAttribute()]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Reason", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Token", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)32, (Int32)32, Message = \"Token must be 32 characters long\")]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(AccessToken));

        }

        #endregion Reflection of Database.	
				
    }
}