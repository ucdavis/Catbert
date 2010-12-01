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
   
        #region AuditAction Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the AuditAction with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionWithNullValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditAction = null;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditAction: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the AuditAction with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionWithEmptyStringDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditAction = string.Empty;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditAction: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the AuditAction with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionWithSpacesOnlyDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditAction = " ";
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditAction: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the AuditAction with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditAction = "x".RepeatTimes((1 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(1 + 1, audit.AuditAction.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditAction: length must be between 0 and 1");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the AuditAction with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAuditActionWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.AuditAction = "x";
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
        /// Tests the AuditAction with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAuditActionWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.AuditAction = "x".RepeatTimes(1);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, audit.AuditAction.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion AuditAction Tests

        #region Username Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Username with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithNullValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = null;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Username with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithEmptyStringDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = string.Empty;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Username with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithSpacesOnlyDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = " ";
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: may not be null or empty");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Username with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = "x".RepeatTimes((256 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(256 + 1, audit.Username.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: length must be between 0 and 256");
                //Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Username with one character saves.
        /// </summary>
        [TestMethod]
        public void TestUsernameWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.Username = "x";
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
        /// Tests the Username with long value saves.
        /// </summary>
        [TestMethod]
        public void TestUsernameWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.Username = "x".RepeatTimes(256);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit, true);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(256, audit.Username.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Username Tests

        #region AuditDate Tests

        /// <summary>
        /// Tests the AuditDate with past date will save.
        /// </summary>
        [TestMethod]
        public void TestAuditDateWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Audit record = GetValid(99);
            record.AuditDate = compareDate;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(record, true);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.AuditDate);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the AuditDate with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestAuditDateWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.AuditDate = compareDate;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(record, true);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.AuditDate);
            #endregion Assert
        }

        /// <summary>
        /// Tests the AuditDate with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestAuditDateWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.AuditDate = compareDate;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(record, true);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.AuditDate);
            #endregion Assert
        }
        #endregion AuditDate Tests

        #region SetActionCode Tests

        [TestMethod]
        public void TestSetActionCodeSetsCorrectlyForCreate()
        {
            #region Arrange
            var audit = new Audit();
            #endregion Arrange

            #region Act
            audit.SetActionCode(AuditActionType.Create);
            #endregion Act

            #region Assert
            Assert.AreEqual("C", audit.AuditAction);
            #endregion Assert		
        }

        [TestMethod]
        public void TestSetActionCodeSetsCorrectlyForUpdate()
        {
            #region Arrange
            var audit = new Audit();
            #endregion Arrange

            #region Act
            audit.SetActionCode(AuditActionType.Update);
            #endregion Act

            #region Assert
            Assert.AreEqual("U", audit.AuditAction);
            #endregion Assert
        }

        [TestMethod]
        public void TestSetActionCodeSetsCorrectlyForDelete()
        {
            #region Arrange
            var audit = new Audit();
            #endregion Arrange

            #region Act
            audit.SetActionCode(AuditActionType.Delete);
            #endregion Act

            #region Assert
            Assert.AreEqual("D", audit.AuditAction);
            #endregion Assert
        }
        #endregion SetActionCode Tests

        #region Guid Tests

        [TestMethod]
        public void TestAuditSavesWithNewGuid()
        {
            #region Arrange
            var audit = CreateValidEntities.Audit(1, true);
            Assert.AreEqual(Guid.Empty, audit.Id);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            Assert.AreNotEqual(Guid.Empty, audit.Id);
            #endregion Assert		
        }
        #endregion Guid Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapAudit1()
        {
            #region Arrange            
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Audit>(session, new AuditEqualityComparer())
                .CheckProperty(c => c.Id, Guid.Empty) 
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapAudit2()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Audit>(session)
                .CheckProperty(c => c.AuditAction, "C")
                .CheckProperty(c => c.AuditDate, new DateTime(2010, 11, 30, 13, 03, 05, 00))
                .CheckProperty(c => c.ObjectId, "ObjectId")
                .CheckProperty(c => c.ObjectName, "ObjectName")
                .CheckProperty(c => c.Username, "UserName")
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
            expectedFields.Add(new NameAndType("AuditAction", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("AuditDate", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Guid", new List<string>
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
            expectedFields.Add(new NameAndType("Username", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)256)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Audit));

        }

        #endregion Reflection of Database.	
		
        public class AuditEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is Guid && y is Guid)
                {
                    var xVal = (Guid)x;
                    var yVal = (Guid)y;                    
                    Assert.AreNotEqual(xVal, yVal);
                    Assert.IsInstanceOfType(x, typeof(Guid));
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