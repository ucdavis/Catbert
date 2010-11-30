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
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRepositoryTests"/> class.
        /// </summary>
        public ApplicationRepositoryTests()
        {
            ApplicationRepository = new Repository<Application>();
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

        #region WebServiceHash Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the WebServiceHash with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestWebServiceHashWithTooLongValueDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.WebServiceHash = "x".RepeatTimes((100 + 1));
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
                Assert.AreEqual(100 + 1, application.WebServiceHash.Length);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("WebServiceHash: length must be between 0 and 100");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the WebServiceHash with null value saves.
        /// </summary>
        [TestMethod]
        public void TestWebServiceHashWithNullValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.WebServiceHash = null;
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
        /// Tests the WebServiceHash with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestWebServiceHashWithEmptyStringSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.WebServiceHash = string.Empty;
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
        /// Tests the WebServiceHash with one space saves.
        /// </summary>
        [TestMethod]
        public void TestWebServiceHashWithOneSpaceSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.WebServiceHash = " ";
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
        /// Tests the WebServiceHash with one character saves.
        /// </summary>
        [TestMethod]
        public void TestWebServiceHashWithOneCharacterSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.WebServiceHash = "x";
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
        /// Tests the WebServiceHash with long value saves.
        /// </summary>
        [TestMethod]
        public void TestWebServiceHashWithLongValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.WebServiceHash = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, application.WebServiceHash.Length);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion WebServiceHash Tests

        #region Salt Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Salt with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestSaltWithTooLongValueDoesNotSave()
        {
            Application application = null;
            try
            {
                #region Arrange
                application = GetValid(9);
                application.Salt = "x".RepeatTimes((20 + 1));
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
                Assert.AreEqual(20 + 1, application.Salt.Length);
                var results = application.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Salt: length must be between 0 and 20");
                Assert.IsTrue(application.IsTransient());
                Assert.IsFalse(application.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Salt with null value saves.
        /// </summary>
        [TestMethod]
        public void TestSaltWithNullValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Salt = null;
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
        /// Tests the Salt with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestSaltWithEmptyStringSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Salt = string.Empty;
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
        /// Tests the Salt with one space saves.
        /// </summary>
        [TestMethod]
        public void TestSaltWithOneSpaceSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Salt = " ";
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
        /// Tests the Salt with one character saves.
        /// </summary>
        [TestMethod]
        public void TestSaltWithOneCharacterSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Salt = "x";
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
        /// Tests the Salt with long value saves.
        /// </summary>
        [TestMethod]
        public void TestSaltWithLongValueSaves()
        {
            #region Arrange
            var application = GetValid(9);
            application.Salt = "x".RepeatTimes(20);
            #endregion Arrange

            #region Act
            ApplicationRepository.DbContext.BeginTransaction();
            ApplicationRepository.EnsurePersistent(application);
            ApplicationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(20, application.Salt.Length);
            Assert.IsFalse(application.IsTransient());
            Assert.IsTrue(application.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Salt Tests

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

        //TODO: List tests
        //Mapping tests (Salt and WebServiceHash?)
        //Constructor tests
        //All fields filled out?
        
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
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Inactive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Location", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)256)]"
            }));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Salt", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)20)]"
            }));
            expectedFields.Add(new NameAndType("WebServiceHash", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Application));

        }

        #endregion Reflection of Database.	
		
		
    }
}