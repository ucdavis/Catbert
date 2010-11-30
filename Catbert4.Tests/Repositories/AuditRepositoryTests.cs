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
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Catbert4.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		Audit
    /// LookupFieldName:	ObjectName
    /// </summary>
    [TestClass]
    public class AuditRepositoryTests : AbstractRepositoryTests<Audit, Guid, AuditMap>
    {
        /// <summary>
        /// Gets or sets the Audit repository.
        /// </summary>
        /// <value>The Audit repository.</value>
        public IRepository<Audit> AuditRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRepositoryTests"/> class.
        /// </summary>
        public AuditRepositoryTests()
        {
            AuditRepository = new Repository<Audit>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Audit GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.Audit(counter);
            var localCounter = 99;
            if (counter != null)
            {
                localCounter = (int)counter;
            }
            rtValue.SetIdTo(SpecificGuid.GetGuid(localCounter));
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Audit> GetQuery(int numberAtEnd)
        {
            return AuditRepository.Queryable.Where(a => a.ObjectName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Audit entity, int counter)
        {
            Assert.AreEqual("ObjectName" + counter, entity.ObjectName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Audit entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.ObjectName);
                    break;
                case ARTAction.Restore:
                    entity.ObjectName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.ObjectName;
                    entity.ObjectName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            AuditRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            AuditRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region ObjectName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ObjectName with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithNullValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = null;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit, true);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ObjectName with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithEmptyStringDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = string.Empty;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit, true);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ObjectName with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithSpacesOnlyDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = " ";
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit, true);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ObjectName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit, true);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(50 + 1, audit.ObjectName.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: length must be between 0 and 50");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ObjectName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestObjectNameWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectName = "x";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestObjectNameWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, audit.ObjectName.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ObjectName Tests
   
        #region ObjectId Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ObjectId with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectIdWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectId = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit, true);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(50 + 1, audit.ObjectId.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectId: length must be between 0 and 50");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ObjectId with null value saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithNullValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = null;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithEmptyStringSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = string.Empty;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with one space saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithOneSpaceSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = " ";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = "x";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, audit.ObjectId.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ObjectId Tests
   
        
        
        
        
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
            expectedFields.Add(new NameAndType("ObjectId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("ObjectName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Audit));

        }

        #endregion Reflection of Database.	
		
		
    }
}