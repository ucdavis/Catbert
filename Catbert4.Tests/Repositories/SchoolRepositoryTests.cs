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
    /// Entity Name:		School
    /// LookupFieldName:	Abbreviation yrjuy
    /// </summary>
    [TestClass]
    public class SchoolRepositoryTests : AbstractRepositoryTests<School, string , SchoolMap>
    {
        /// <summary>
        /// Gets or sets the School repository.
        /// </summary>
        /// <value>The School repository.</value>
        public IRepositoryWithTypedId<School, string > SchoolRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolRepositoryTests"/> class.
        /// </summary>
        public SchoolRepositoryTests()
        {
            SchoolRepository = new RepositoryWithTypedId<School, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override School GetValid(int? counter)
        {
            return CreateValidEntities.School(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<School> GetQuery(int numberAtEnd)
        {
            return SchoolRepository.Queryable.Where(a => a.ShortDescription.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(School entity, int counter)
        {
            Assert.AreEqual("ShortDescription" + counter, entity.ShortDescription);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(School entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.ShortDescription);
                    break;
                case ARTAction.Restore:
                    entity.ShortDescription = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.ShortDescription;
                    entity.ShortDescription = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            SchoolRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            SchoolRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region ShortDescription Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ShortDescription with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortDescriptionWithNullValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.ShortDescription = null;
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortDescription: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ShortDescription with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortDescriptionWithEmptyStringDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.ShortDescription = string.Empty;
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortDescription: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ShortDescription with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortDescriptionWithSpacesOnlyDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.ShortDescription = " ";
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortDescription: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ShortDescription with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortDescriptionWithTooLongValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.ShortDescription = "x".RepeatTimes((25 + 1));
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                Assert.AreEqual(25 + 1, school.ShortDescription.Length);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortDescription: length must be between 0 and 25");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ShortDescription with one character saves.
        /// </summary>
        [TestMethod]
        public void TestShortDescriptionWithOneCharacterSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.ShortDescription = "x";
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ShortDescription with long value saves.
        /// </summary>
        [TestMethod]
        public void TestShortDescriptionWithLongValueSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.ShortDescription = "x".RepeatTimes(25);
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(25, school.ShortDescription.Length);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ShortDescription Tests

        #region LongDescription Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the LongDescription with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLongDescriptionWithNullValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.LongDescription = null;
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LongDescription: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LongDescription with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLongDescriptionWithEmptyStringDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.LongDescription = string.Empty;
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LongDescription: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LongDescription with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLongDescriptionWithSpacesOnlyDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.LongDescription = " ";
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LongDescription: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LongDescription with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLongDescriptionWithTooLongValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.LongDescription = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                Assert.AreEqual(50 + 1, school.LongDescription.Length);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LongDescription: length must be between 0 and 50");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LongDescription with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLongDescriptionWithOneCharacterSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.LongDescription = "x";
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LongDescription with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLongDescriptionWithLongValueSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.LongDescription = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, school.LongDescription.Length);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LongDescription Tests

        #region Abbreviation Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Abbreviation with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAbbreviationWithNullValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.Abbreviation = null;
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Abbreviation: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Abbreviation with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAbbreviationWithEmptyStringDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.Abbreviation = string.Empty;
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Abbreviation: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Abbreviation with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAbbreviationWithSpacesOnlyDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.Abbreviation = " ";
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Abbreviation: may not be null or empty");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Abbreviation with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAbbreviationWithTooLongValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.Abbreviation = "x".RepeatTimes((12 + 1));
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(school);
                Assert.AreEqual(12 + 1, school.Abbreviation.Length);
                var results = school.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Abbreviation: length must be between 0 and 12");
                //Assert.IsTrue(school.IsTransient());
                Assert.IsFalse(school.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Abbreviation with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAbbreviationWithOneCharacterSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Abbreviation = "x";
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Abbreviation with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAbbreviationWithLongValueSaves()
        {
            #region Arrange
            var school = GetValid(9);
            school.Abbreviation = "x".RepeatTimes(12);
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(12, school.Abbreviation.Length);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Abbreviation Tests

        #region Units Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Units with A value of xxx does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestUnitsWithNewUnitValueDoesNotSave()
        {
            School school = null;
            try
            {
                #region Arrange
                school = GetValid(9);
                school.Units.Add(CreateValidEntities.Unit(9));
                #endregion Arrange

                #region Act
                SchoolRepository.DbContext.BeginTransaction();
                SchoolRepository.EnsurePersistent(school);
                SchoolRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(school);
                Assert.IsNotNull(ex);
                Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
                throw;
            }	
        }
        #endregion Invalid Tests
        #region Valid Tests

        [TestMethod]
        public void TestSchoolWithANullUnitsWillSave()
        {
            #region Arrange
            var school = GetValid(9);
            school.Units = null;
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(school.Units);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestSchoolWithAnEmptyUnitsWillSave()
        {
            #region Arrange
            var school = GetValid(9);
            school.Units = new List<Unit>();
            #endregion Arrange

            #region Act
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(school.Units);
            Assert.AreEqual(0, school.Units.Count);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestSchoolWithExistingUnitsWillSave()
        {
            #region Arrange
            var school = GetValid(9);
            school.Units = new List<Unit>();
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();

            Repository.OfType<Unit>().DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var unit = CreateValidEntities.Unit(i + 1);
                unit.School = school;
                Repository.OfType<Unit>().EnsurePersistent(unit);
            }
            Repository.OfType<Unit>().DbContext.CommitTransaction();

            var saveId = school.Id;
            #endregion Arrange

            #region Act

            NHibernateSessionManager.Instance.GetSession().Evict(school);
            school = SchoolRepository.GetNullableById(saveId);
            Assert.IsNotNull(school);
            Assert.IsNotNull(school.Units);
            Assert.AreEqual(3, school.Units.Count);
            school.ShortDescription = "Updated";
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(school.Units);
            Assert.AreEqual(3, school.Units.Count);
            Assert.AreEqual("Updated", school.ShortDescription);
            Assert.IsFalse(school.IsTransient());
            Assert.IsTrue(school.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        public void TestDeleteSchoolDoesNotCascadeToUnit()
        {
            #region Arrange
            var school = GetValid(9);
            school.Units = new List<Unit>();
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.EnsurePersistent(school);
            SchoolRepository.DbContext.CommitTransaction();

            Repository.OfType<Unit>().DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var unit = CreateValidEntities.Unit(i + 1);
                unit.School = school;
                Repository.OfType<Unit>().EnsurePersistent(unit);
            }
            Repository.OfType<Unit>().DbContext.CommitTransaction();

            var saveId = school.Id;
            #endregion Arrange

            #region Act

            NHibernateSessionManager.Instance.GetSession().Evict(school);
            school = SchoolRepository.GetNullableById(saveId);
            Assert.IsNotNull(school);
            Assert.IsNotNull(school.Units);
            Assert.AreEqual(3, school.Units.Count);
            SchoolRepository.DbContext.BeginTransaction();
            SchoolRepository.Remove(school);
            SchoolRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(3, Repository.OfType<Unit>().Queryable.Count());
            Assert.IsNull(SchoolRepository.GetNullableById(saveId));
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Units Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapSchool1()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<School>(session)
                .CheckProperty(c => c.Id, "Id")
                .CheckProperty(c => c.Abbreviation, "Abbreviation")
                .CheckProperty(c => c.LongDescription, "LongDescription")
                .CheckProperty(c => c.ShortDescription, "ShortDescription")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapSchool2()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            var school = new School();
            school.SetIdTo("Id");
            Repository.OfType<Unit>().DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var unit = CreateValidEntities.Unit(i + 1);
                unit.School = school;
                Repository.OfType<Unit>().EnsurePersistent(unit);
            }
            Repository.OfType<Unit>().DbContext.CommitTransaction();

            IList<Unit> units = Repository.OfType<Unit>().Queryable.Where(a => a.School.Id == "Id").ToList();
            Assert.AreEqual(3, units.Count);

            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<School>(session, new SchoolEqualityComparer())
                .CheckProperty(c => c.Id, "Id")
                .CheckProperty(c => c.Abbreviation, "Abbreviation")
                .CheckProperty(c => c.LongDescription, "LongDescription")
                .CheckProperty(c => c.ShortDescription, "ShortDescription")
                .CheckProperty(c => c.Units, units)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class SchoolEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is IList<Unit> && y is IList<Unit>)
                {
                    var xVal = (IList<Unit>)x;
                    var yVal = (IList<Unit>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
                        Assert.AreEqual(xVal[i].FullName, yVal[i].FullName);
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
            expectedFields.Add(new NameAndType("Abbreviation", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)12)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[NHibernate.Validator.Constraints.LengthAttribute((Int32)2)]", 
                "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("LongDescription", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ShortDescription", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)25)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Units", "System.Collections.Generic.IList`1[Catbert4.Core.Domain.Unit]", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(School));

        }

        #endregion Reflection of Database.	
		
    }
}