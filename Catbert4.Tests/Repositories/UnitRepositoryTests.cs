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
    /// Entity Name:		Unit
    /// LookupFieldName:	FullName yrjuy
    /// </summary>
    [TestClass]
    public class UnitRepositoryTests : AbstractRepositoryTests<Unit, int, UnitMap>
    {
        /// <summary>
        /// Gets or sets the Unit repository.
        /// </summary>
        /// <value>The Unit repository.</value>
        public IRepository<Unit> UnitRepository { get; set; }
        public IRepositoryWithTypedId<School, string> SchoolRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitRepositoryTests"/> class.
        /// </summary>
        public UnitRepositoryTests()
        {
            UnitRepository = new Repository<Unit>();
            SchoolRepository = new RepositoryWithTypedId<School, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Unit GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.Unit(counter);
            rtValue.School = SchoolRepository.GetById("1");
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Unit> GetQuery(int numberAtEnd)
        {
            return UnitRepository.Queryable.Where(a => a.FullName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Unit entity, int counter)
        {
            Assert.AreEqual("FullName" + counter, entity.FullName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Unit entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.FullName);
                    break;
                case ARTAction.Restore:
                    entity.FullName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.FullName;
                    entity.FullName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            UnitRepository.DbContext.BeginTransaction();
            base.LoadSchools(3);
            LoadRecords(5);
            UnitRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region FullName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the FullName with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFullNameWithNullValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FullName = null;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FullName: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FullName with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFullNameWithEmptyStringDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FullName = string.Empty;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FullName: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FullName with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFullNameWithSpacesOnlyDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FullName = " ";
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FullName: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FullName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFullNameWithTooLongValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FullName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                Assert.AreEqual(50 + 1, unit.FullName.Length);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FullName: length must be between 0 and 50");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the FullName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestFullNameWithOneCharacterSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.FullName = "x";
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FullName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestFullNameWithLongValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.FullName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, unit.FullName.Length);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion FullName Tests
        
        #region ShortName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ShortName with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortNameWithNullValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.ShortName = null;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortName: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ShortName with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortNameWithEmptyStringDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.ShortName = string.Empty;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortName: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ShortName with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortNameWithSpacesOnlyDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.ShortName = " ";
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortName: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ShortName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestShortNameWithTooLongValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.ShortName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                Assert.AreEqual(50 + 1, unit.ShortName.Length);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ShortName: length must be between 0 and 50");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ShortName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestShortNameWithOneCharacterSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.ShortName = "x";
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ShortName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestShortNameWithLongValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.ShortName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, unit.ShortName.Length);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ShortName Tests

        #region PpsCode Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the PpsCode with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPpsCodeWithTooLongValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.PpsCode = "x".RepeatTimes((6 + 1));
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                Assert.AreEqual(6 + 1, unit.PpsCode.Length);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("PpsCode: length must be between 0 and 6");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the PpsCode with null value saves.
        /// </summary>
        [TestMethod]
        public void TestPpsCodeWithNullValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.PpsCode = null;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the PpsCode with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestPpsCodeWithEmptyStringSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.PpsCode = string.Empty;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the PpsCode with one space saves.
        /// </summary>
        [TestMethod]
        public void TestPpsCodeWithOneSpaceSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.PpsCode = " ";
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the PpsCode with one character saves.
        /// </summary>
        [TestMethod]
        public void TestPpsCodeWithOneCharacterSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.PpsCode = "x";
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the PpsCode with long value saves.
        /// </summary>
        [TestMethod]
        public void TestPpsCodeWithLongValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.PpsCode = "x".RepeatTimes(6);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(6, unit.PpsCode.Length);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion PpsCode Tests
      
        #region FisCode Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the FisCode with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFisCodeWithNullValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FisCode = null;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FisCode: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FisCode with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFisCodeWithEmptyStringDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FisCode = string.Empty;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FisCode: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FisCode with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFisCodeWithSpacesOnlyDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FisCode = " ";
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FisCode: may not be null or empty");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FisCode with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFisCodeWithTooLongValueDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.FisCode = "x".RepeatTimes((4 + 1));
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                Assert.AreEqual(4 + 1, unit.FisCode.Length);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FisCode: length must be between 0 and 4");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the FisCode with one character saves.
        /// </summary>
        [TestMethod]
        public void TestFisCodeWithOneCharacterSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.FisCode = "x";
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FisCode with long value saves.
        /// </summary>
        [TestMethod]
        public void TestFisCodeWithLongValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.FisCode = "x".RepeatTimes(4);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(4, unit.FisCode.Length);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion FisCode Tests

        #region School Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUnitWithNullSchoolDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.School = null;
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(unit);
                var results = unit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("School: may not be null");
                Assert.IsTrue(unit.IsTransient());
                Assert.IsFalse(unit.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestUnitWithNewSchoolDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.School = new School();
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unit);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.School, Entity: Catbert4.Core.Domain.School", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestUnitSavesWithAnExistingSchool()
        {
            #region Arrange
            var unit = GetValid(9);
            var school = SchoolRepository.GetById("2");
            unit.School = school;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(school, unit.School);
            Assert.AreEqual("ShortDescription2", unit.School.ShortDescription);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        public void TestUnitWithUnsavedSchoolDoesNotSaveCascade()
        {
            #region Arrange
            var schoolCount = SchoolRepository.Queryable.Count();
            var unit = GetValid(9);
            unit.School = CreateValidEntities.School(99);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(schoolCount, SchoolRepository.Queryable.Count());
            #endregion Assert
        }

        [TestMethod]
        public void TestUnitDeleteDoesNotCascadedToSchool()
        {
            #region Arrange
            var schoolCount = SchoolRepository.Queryable.Count();
            Assert.IsTrue(schoolCount > 0);
            var unitCount = UnitRepository.Queryable.Count();
            var unit = UnitRepository.GetById(2);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.Remove(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(schoolCount, SchoolRepository.Queryable.Count());
            Assert.AreEqual(unitCount - 1, UnitRepository.Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Unit Tests

        #region Parent Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Parent with A value of new unit does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestParentWithAValueOfNewDoesNotSave()
        {
            Unit unit = null;
            try
            {
                #region Arrange
                unit = GetValid(9);
                unit.Parent = CreateValidEntities.Unit(99);
                #endregion Arrange

                #region Act
                UnitRepository.DbContext.BeginTransaction();
                UnitRepository.EnsurePersistent(unit);
                UnitRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(unit);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Catbert4.Core.Domain.Unit, Entity: Catbert4.Core.Domain.Unit", ex.Message);
                throw;
            }	
        }
        
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestParentWithNullValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.Parent = null;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(unit.Parent);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestParentWithExistingValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.Parent = UnitRepository.GetNullableById(1);
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(unit.Parent);
            Assert.AreEqual(1, unit.Parent.Id);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestParentDoesNotCascadeDeleteWhenChildIsDeleted()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.Parent = UnitRepository.GetNullableById(1);

            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            var saveId = unit.Id;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.Remove(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(UnitRepository.GetNullableById(saveId));
            Assert.IsNotNull(UnitRepository.GetNullableById(1));
            #endregion Assert		
        }

        [TestMethod]
        public void TestChildDoesNotCascadeDeleteWhenParentIsDeleted()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.Parent = UnitRepository.GetNullableById(1);

            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            var saveId = unit.Id;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.Remove(UnitRepository.GetNullableById(1));
            UnitRepository.DbContext.CommitTransaction();
            NHibernateSessionManager.Instance.GetSession().Evict(unit);
            unit = UnitRepository.GetNullableById(saveId);
            #endregion Act

            #region Assert
            Assert.IsNotNull(unit);
            //Assert.IsNull(unit.Parent); //Don't care, it would be done in the DB.
            Assert.IsNull(UnitRepository.GetNullableById(1));
            #endregion Assert
        }
        #endregion Cascade Tests

        #endregion Parent Tests

        #region Type Tests

        [TestMethod]
        public void TestTypeWithValidValueSaves1()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.Type = UnitType.Cluster;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(UnitType.Cluster, unit.Type);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert	
        }
        [TestMethod]
        public void TestTypeWithValidValueSaves2()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.Type = UnitType.Department;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(UnitType.Department, unit.Type);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestTypeDefaultValue()
        {
            #region Arrange
            var unit = new Unit();
            //Defaults to Department
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(UnitType.Department, unit.Type);
            #endregion Assert
        }
        
        #endregion Type Tests

        #region UnitAssociations Tests

        #region Valid Tests

        [TestMethod]
        public void TestUnitAssoicationsWithNullValueSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.UnitAssociations = null;
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(unit.UnitAssociations);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestUnitAssoicationsWithEmptyListSaves()
        {
            #region Arrange
            var unit = GetValid(9);
            unit.UnitAssociations = new List<UnitAssociation>();
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(unit.UnitAssociations);
            Assert.AreEqual(0,unit.UnitAssociations.Count);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestUnitAssoicationsWithExistingValuesSaves()
        {
            #region Arrange
            var unit = UnitRepository.GetNullableById(1);

            Repository.OfType<UnitAssociation>().DbContext.BeginTransaction();
            LoadUsers(1);
            LoadApplications(1);
            LoadUnitAssociations(3, Repository.OfType<User>().Queryable.First());
            Repository.OfType<UnitAssociation>().DbContext.CommitTransaction();

            NHibernateSessionManager.Instance.GetSession().Evict(unit);
            unit = UnitRepository.GetNullableById(1);

            Assert.AreEqual(3, unit.UnitAssociations.Count());
            #endregion Arrange

            #region Act
            unit.ShortName = "Updated";
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.EnsurePersistent(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("Updated", unit.ShortName);
            Assert.IsNotNull(unit.UnitAssociations);
            Assert.AreEqual(3, unit.UnitAssociations.Count);
            Assert.IsFalse(unit.IsTransient());
            Assert.IsTrue(unit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #region Cascade Tests

        [TestMethod]
        public void TestDeleteUnitDoesNotCascadeToUnitAssociation()
        {
            #region Arrange
            var unit = UnitRepository.GetNullableById(1);

            Repository.OfType<UnitAssociation>().DbContext.BeginTransaction();
            LoadUsers(1);
            LoadApplications(1);
            LoadUnitAssociations(3, Repository.OfType<User>().Queryable.First());
            Repository.OfType<UnitAssociation>().DbContext.CommitTransaction();
            var count = Repository.OfType<UnitAssociation>().Queryable.Count();
            Assert.IsTrue(count > 0);

            NHibernateSessionManager.Instance.GetSession().Evict(unit);
            unit = UnitRepository.GetNullableById(1);

            Assert.AreEqual(3, unit.UnitAssociations.Count());
            #endregion Arrange

            #region Act
            UnitRepository.DbContext.BeginTransaction();
            UnitRepository.Remove(unit);
            UnitRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(UnitRepository.GetNullableById(1));
            Assert.AreEqual(count, Repository.OfType<UnitAssociation>().Queryable.Count());
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion UnitAssociations Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapUnit1()
        {
            #region Arrange
            var id = UnitRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var school = SchoolRepository.GetById("2");
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Unit>(session, new UnitEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.FisCode, "FIS")
                .CheckProperty(c => c.FullName, "FullName")
                .CheckProperty(c => c.PpsCode, "PPS")
                .CheckProperty(c => c.ShortName, "ShortName")
                .CheckProperty(c => c.School, school)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapUnit2()
        {
            #region Arrange
            var id = UnitRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var school = SchoolRepository.GetById("2");
            var parent = UnitRepository.GetNullableById(1);
            Assert.IsNotNull(parent);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Unit>(session, new UnitEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.FisCode, "FIS")
                .CheckProperty(c => c.FullName, "FullName")
                .CheckProperty(c => c.PpsCode, "PPS")
                .CheckProperty(c => c.ShortName, "ShortName")
                .CheckProperty(c => c.School, school)
                .CheckReference(c => c.Parent, parent)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapUnit3()
        {
            #region Arrange
            var id = UnitRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var school = SchoolRepository.GetById("2");
            var parent = UnitRepository.GetNullableById(1);
            Assert.IsNotNull(parent);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Unit>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.FisCode, "FIS")
                .CheckProperty(c => c.FullName, "FullName")
                .CheckProperty(c => c.PpsCode, "PPS")
                .CheckProperty(c => c.ShortName, "ShortName")
                .CheckProperty(c => c.Type, UnitType.Cluster)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapUnit4()
        {
            #region Arrange
            var id = UnitRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var school = SchoolRepository.GetById("2");
            var parent = UnitRepository.GetNullableById(1);
            Assert.IsNotNull(parent);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Unit>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.FisCode, "FIS")
                .CheckProperty(c => c.FullName, "FullName")
                .CheckProperty(c => c.PpsCode, "PPS")
                .CheckProperty(c => c.ShortName, "ShortName")
                .CheckProperty(c => c.Type, UnitType.Department)
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
            expectedFields.Add(new NameAndType("FisCode", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)4)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("FullName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Parent", "Catbert4.Core.Domain.Unit", new List<string>()));
            expectedFields.Add(new NameAndType("PpsCode", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)6)]"
            }));
            expectedFields.Add(new NameAndType("School", "Catbert4.Core.Domain.School", new List<string>
            {
                 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ShortName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Type", "Catbert4.Core.Domain.UnitType", new List<string>()));
            expectedFields.Add(new NameAndType("UnitAssociations", "System.Collections.Generic.IList`1[Catbert4.Core.Domain.UnitAssociation]", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Unit));

        }

        #endregion Reflection of Database.	
		
        public class UnitEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is School && y is School)
                {
                    var xVal = (School)x;
                    var yVal = (School)y;
                    Assert.AreEqual(xVal.ShortDescription, yVal.ShortDescription);
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