using System;
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
using UCDArch.Testing.Extensions;

namespace Catbert4.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		Message
    /// LookupFieldName:	Text
    /// </summary>
    [TestClass]
    public class MessageRepositoryTests : AbstractRepositoryTests<Message, int, MessageMap>
    {
        /// <summary>
        /// Gets or sets the Message repository.
        /// </summary>
        /// <value>The Message repository.</value>
        public IRepository<Message> MessageRepository { get; set; }
        public IRepository<Application> ApplicationRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepositoryTests"/> class.
        /// </summary>
        public MessageRepositoryTests()
        {
            MessageRepository = new Repository<Message>();
            ApplicationRepository = new Repository<Application>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Message GetValid(int? counter)
        {
            return CreateValidEntities.Message(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Message> GetQuery(int numberAtEnd)
        {
            return MessageRepository.Queryable.Where(a => a.Text.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Message entity, int counter)
        {
            Assert.AreEqual("Text" + counter, entity.Text);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Message entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Text);
                    break;
                case ARTAction.Restore:
                    entity.Text = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Text;
                    entity.Text = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            MessageRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            MessageRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Text Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Text with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTextWithNullValueDoesNotSave()
        {
            Message message = null;
            try
            {
                #region Arrange
                message = GetValid(9);
                message.Text = null;
                #endregion Arrange

                #region Act
                MessageRepository.DbContext.BeginTransaction();
                MessageRepository.EnsurePersistent(message);
                MessageRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(message);
                var results = message.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Text: may not be null or empty");
                Assert.IsTrue(message.IsTransient());
                Assert.IsFalse(message.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Text with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTextWithEmptyStringDoesNotSave()
        {
            Message message = null;
            try
            {
                #region Arrange
                message = GetValid(9);
                message.Text = string.Empty;
                #endregion Arrange

                #region Act
                MessageRepository.DbContext.BeginTransaction();
                MessageRepository.EnsurePersistent(message);
                MessageRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(message);
                var results = message.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Text: may not be null or empty");
                Assert.IsTrue(message.IsTransient());
                Assert.IsFalse(message.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Text with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTextWithSpacesOnlyDoesNotSave()
        {
            Message message = null;
            try
            {
                #region Arrange
                message = GetValid(9);
                message.Text = " ";
                #endregion Arrange

                #region Act
                MessageRepository.DbContext.BeginTransaction();
                MessageRepository.EnsurePersistent(message);
                MessageRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(message);
                var results = message.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Text: may not be null or empty");
                Assert.IsTrue(message.IsTransient());
                Assert.IsFalse(message.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Text with one character saves.
        /// </summary>
        [TestMethod]
        public void TestTextWithOneCharacterSaves()
        {
            #region Arrange
            var message = GetValid(9);
            message.Text = "x";
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Text with long value saves.
        /// </summary>
        [TestMethod]
        public void TestTextWithLongValueSaves()
        {
            #region Arrange
            var message = GetValid(9);
            message.Text = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, message.Text.Length);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Text Tests

        #region BeginDisplayDate Tests

        /// <summary>
        /// Tests the BeginDisplayDate with past date will save.
        /// </summary>
        [TestMethod]
        public void TestBeginDisplayDateWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Message record = GetValid(99);
            record.BeginDisplayDate = compareDate;
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(record);
            MessageRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.BeginDisplayDate);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the BeginDisplayDate with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestBeginDisplayDateWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.BeginDisplayDate = compareDate;
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(record);
            MessageRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.BeginDisplayDate);
            #endregion Assert
        }

        /// <summary>
        /// Tests the BeginDisplayDate with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestBeginDisplayDateWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.BeginDisplayDate = compareDate;
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(record);
            MessageRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.BeginDisplayDate);
            #endregion Assert
        }
        #endregion BeginDisplayDate Tests
        
        #region EndDisplayDate Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the EndDisplayDate with A value of CurrentDate does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEndDisplayDateWithAValueOfCurrentDateDoesNotSave()
        {
            Message message = null;
            try
            {
                #region Arrange
                message = GetValid(9);
                message.EndDisplayDate = DateTime.Now;
                #endregion Arrange

                #region Act
                MessageRepository.DbContext.BeginTransaction();
                MessageRepository.EnsurePersistent(message);
                MessageRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(message);
                var results = message.ValidationResults().AsMessageList();
                results.AssertErrorsAre("EndDisplayDate: must be a future date");
                Assert.IsTrue(message.IsTransient());
                Assert.IsFalse(message.IsValid());
                throw;
            }	
        }

        /// <summary>
        /// Tests the EndDisplayDate with A value of PastDate does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEndDisplayDateWithAValueOfPastDateDoesNotSave()
        {
            Message message = null;
            try
            {
                #region Arrange
                message = GetValid(9);
                message.EndDisplayDate = DateTime.Now.AddDays(-1);
                #endregion Arrange

                #region Act
                MessageRepository.DbContext.BeginTransaction();
                MessageRepository.EnsurePersistent(message);
                MessageRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(message);
                var results = message.ValidationResults().AsMessageList();
                results.AssertErrorsAre("EndDisplayDate: must be a future date");
                Assert.IsTrue(message.IsTransient());
                Assert.IsFalse(message.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests
        #region Valid Tests

        /// <summary>
        /// Tests the EndDisplayDate with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestEndDisplayDateWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.EndDisplayDate = compareDate;
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(record);
            MessageRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.EndDisplayDate);
            #endregion Assert
        }

        [TestMethod]
        public void TestEndDisplayDateWithFutureDateTimeWillSave()
        {
            #region Arrange

            var compareDate = DateTime.Now.AddMinutes(5);
            var record = GetValid(99);
            record.EndDisplayDate = compareDate;
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(record);
            MessageRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.EndDisplayDate);
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion EndDisplayDate Tests

        #region Critical Tests

        /// <summary>
        /// Tests the Critical is false saves.
        /// </summary>
        [TestMethod]
        public void TestCriticalIsFalseSaves()
        {
            #region Arrange

            Message message = GetValid(9);
            message.Critical = false;

            #endregion Arrange

            #region Act

            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(message.Critical);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Critical is true saves.
        /// </summary>
        [TestMethod]
        public void TestCriticalIsTrueSaves()
        {
            #region Arrange

            var message = GetValid(9);
            message.Critical = true;

            #endregion Arrange

            #region Act

            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(message.Critical);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());

            #endregion Assert
        }

        #endregion Critical Tests

        #region Active Tests

        /// <summary>
        /// Tests the Active is false saves.
        /// </summary>
        [TestMethod]
        public void TestActiveIsFalseSaves()
        {
            #region Arrange

            Message message = GetValid(9);
            message.Active = false;

            #endregion Arrange

            #region Act

            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(message.Active);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Active is true saves.
        /// </summary>
        [TestMethod]
        public void TestActiveIsTrueSaves()
        {
            #region Arrange

            var message = GetValid(9);
            message.Active = true;

            #endregion Arrange

            #region Act

            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(message.Active);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());

            #endregion Assert
        }

        #endregion Active Tests

        #region Application Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Application with A value of new Application does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestApplicationWithAValueOfNewApplicationDoesNotSave()
        {
            Message message = null;
            try
            {
                #region Arrange
                message = GetValid(9);
                message.Application = CreateValidEntities.Application(1);
                #endregion Arrange

                #region Act
                MessageRepository.DbContext.BeginTransaction();
                MessageRepository.EnsurePersistent(message);
                MessageRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(message);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.Application, Entity: Name1", ex.Message);
                throw;
            }	
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestApplicationWithANullValueSaves()
        {
            #region Arrange

            Message message = GetValid(9);
            message.Application = null;

            #endregion Arrange

            #region Act

            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsNull(message.Application);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());

            #endregion Assert		
        }

        [TestMethod]
        public void TestMessageWithExistingApplicationSaves()
        {
            #region Arrange
            ApplicationRepository.DbContext.BeginTransaction();
            LoadApplications(1);
            ApplicationRepository.DbContext.CommitTransaction();
            var message = GetValid(9);
            message.Application = ApplicationRepository.GetById(1);
            #endregion Arrange

            #region Act
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(message.Application);
            Assert.AreEqual("Name1", message.Application.Name);
            Assert.IsFalse(message.IsTransient());
            Assert.IsTrue(message.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestMessageDoesNotCascadeDeleteToApplication()
        {
            #region Arrange
            ApplicationRepository.DbContext.BeginTransaction();
            LoadApplications(1);
            ApplicationRepository.DbContext.CommitTransaction();
            var message = GetValid(9);
            var application = ApplicationRepository.GetNullableById(1);
            Assert.IsNotNull(application);
            message.Application = application;
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.EnsurePersistent(message);
            MessageRepository.DbContext.CommitTransaction();
            var saveId = message.Id;
            NHibernateSessionManager.Instance.GetSession().Evict(application);
            NHibernateSessionManager.Instance.GetSession().Evict(message);
            #endregion Arrange

            #region Act
            message = MessageRepository.GetNullableById(saveId);
            Assert.IsNotNull(message);
            MessageRepository.DbContext.BeginTransaction();
            MessageRepository.Remove(message);
            MessageRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(application);
            NHibernateSessionManager.Instance.GetSession().Evict(message);
            #endregion Act

            #region Assert
            Assert.IsNull(MessageRepository.GetNullableById(saveId));
            Assert.IsNotNull(ApplicationRepository.GetNullableById(1));
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion Application Tests

        #region Constructor Tests

        [TestMethod]
        public void TestConstructorSetsExpectedValues()
        {
            #region Arrange
            var record = new Message();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.Active);
            Assert.AreEqual(DateTime.Now.Date, record.BeginDisplayDate.Date);
            Assert.AreEqual(record.BeginDisplayDate, record.EndDisplayDate);
            #endregion Assert		
        }
        #endregion Constructor Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapMessage1()
        {
            #region Arrange
            var id = MessageRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var compareDate1 = DateTime.Now.Date.AddDays(-10);
            var compareDate2 = DateTime.Now.Date.AddDays(10);
 
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Message>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Active, true)
                .CheckProperty(c => c.BeginDisplayDate, compareDate1)
                .CheckProperty(c => c.Critical, true)
                .CheckProperty(c => c.EndDisplayDate, compareDate2)
                .CheckProperty(c => c.Text, "Text")
                .VerifyTheMappings();
            #endregion Act/Assert
        }
        [TestMethod]
        public void TestCanCorrectlyMapMessage2()
        {
            #region Arrange
            var id = MessageRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var compareDate1 = DateTime.Now.Date.AddDays(-10);
            var compareDate2 = DateTime.Now.Date.AddDays(10);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Message>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Active, false)
                .CheckProperty(c => c.BeginDisplayDate, compareDate1)
                .CheckProperty(c => c.Critical, false)
                .CheckProperty(c => c.EndDisplayDate, compareDate2)
                .CheckProperty(c => c.Text, "Text")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapMessage3()
        {
            #region Arrange
            var id = MessageRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var compareDate1 = DateTime.Now.Date.AddDays(-10);
            var compareDate2 = DateTime.Now.Date.AddDays(10);
            ApplicationRepository.DbContext.BeginTransaction();
            LoadApplications(1);
            ApplicationRepository.DbContext.CommitTransaction();
            var application = ApplicationRepository.GetNullableById(1);
            Assert.IsNotNull(application);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Message>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Active, true)
                .CheckProperty(c => c.BeginDisplayDate, compareDate1)
                .CheckProperty(c => c.Critical, true)
                .CheckProperty(c => c.EndDisplayDate, compareDate2)
                .CheckProperty(c => c.Text, "Text")
                .CheckReference(c => c.Application, application)
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
            expectedFields.Add(new NameAndType("Application", "Catbert4.Core.Domain.Application", new List<string>()));
            expectedFields.Add(new NameAndType("BeginDisplayDate", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Critical", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("EndDisplayDate", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.FutureAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Text", "System.String", new List<string>
            {
                "[System.ComponentModel.DataAnnotations.DataTypeAttribute((System.ComponentModel.DataAnnotations.DataType)9)]", 
                "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"                 
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Message));

        }

        #endregion Reflection of Database.	
		
		
    }
}