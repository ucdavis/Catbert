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
    /// Entity Name:		Permission
    /// LookupFieldName:	Inactive yrjuy
    /// </summary>
    [TestClass]
    public class PermissionRepositoryTests : AbstractRepositoryTests<Permission, int, PermissionMap>
    {
        /// <summary>
        /// Gets or sets the Permission repository.
        /// </summary>
        /// <value>The Permission repository.</value>
        public IRepository<Permission> PermissionRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRepositoryTests"/> class.
        /// </summary>
        public PermissionRepositoryTests()
        {
            PermissionRepository = new Repository<Permission>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Permission GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.Permission(counter);
            rtValue.User = Repository.OfType<User>().GetById(1);
            rtValue.Role = Repository.OfType<Role>().GetById(1);
            rtValue.Application = Repository.OfType<Application>().GetById(1);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Permission> GetQuery(int numberAtEnd)
        {
            return PermissionRepository.Queryable.Where(a => a.Id == numberAtEnd);
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Permission entity, int counter)
        {
            Assert.AreEqual(counter, entity.Id);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Permission entity, ARTAction action)
        {
            
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(Repository.OfType<User>().GetNullableById(2).LoginId, entity.User.LoginId);
                    break;
                case ARTAction.Restore:
                    entity.User = UserRestoreValue;
                    break;
                case ARTAction.Update:
                    UserRestoreValue = entity.User;
                    entity.User = Repository.OfType<User>().GetNullableById(2);
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            PermissionRepository.DbContext.BeginTransaction();
            LoadRoles(3);
            LoadApplications(3);
            LoadRecords(5);
            LoadUsers(3);
            PermissionRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Inactive Tests

        /// <summary>
        /// Tests the Inactive is false saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsFalseSaves()
        {
            #region Arrange

            Permission permission = GetValid(9);
            permission.Inactive = false;

            #endregion Arrange

            #region Act

            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.EnsurePersistent(permission);
            PermissionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(permission.Inactive);
            Assert.IsFalse(permission.IsTransient());
            Assert.IsTrue(permission.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Inactive is true saves.
        /// </summary>
        [TestMethod]
        public void TestInactiveIsTrueSaves()
        {
            #region Arrange

            var permission = GetValid(9);
            permission.Inactive = true;

            #endregion Arrange

            #region Act

            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.EnsurePersistent(permission);
            PermissionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(permission.Inactive);
            Assert.IsFalse(permission.IsTransient());
            Assert.IsTrue(permission.IsValid());

            #endregion Assert
        }

        #endregion Inactive Tests

        #region User Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPermissionWithNullUserDoesNotSave()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.User = null;
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(permission);
                var results = permission.ValidationResults().AsMessageList();
                results.AssertErrorsAre("User: may not be null");
                Assert.IsTrue(permission.IsTransient());
                Assert.IsFalse(permission.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestPermissionWithNewUserDoesNotSave()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.User = new User();
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(permission);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.Permission.User", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestPermissionSavesWithAnExistingUser()
        {
            #region Arrange
            var permission = GetValid(9);
            var user = Repository.OfType<User>().GetById(2);
            permission.User = user;
            #endregion Arrange

            #region Act
            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.EnsurePersistent(permission);
            PermissionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(user, permission.User);
            Assert.AreEqual("LoginId2", permission.User.LoginId);
            Assert.IsFalse(permission.IsTransient());
            Assert.IsTrue(permission.IsValid());
            #endregion Assert		
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestPermissionWithUnsavedUserDoesNotSaveOrCascade()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.User = CreateValidEntities.User(99);
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(permission);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.Permission.User", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestPermissionDeleteDoesNotCascadedToUser()
        {
            #region Arrange
            var userCount = Repository.OfType<User>().Queryable.Count();
            Assert.IsTrue(userCount > 0);
            var permissionCount = PermissionRepository.Queryable.Count();
            var permission = PermissionRepository.GetById(2);
            #endregion Arrange

            #region Act
            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.Remove(permission);
            PermissionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(userCount, Repository.OfType<User>().Queryable.Count());
            Assert.AreEqual(permissionCount - 1, PermissionRepository.Queryable.Count());
            #endregion Assert		
        }
        #endregion Cascade Tests
        #endregion User Tests

        #region Application Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPermissionWithNullApplicationDoesNotSave()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.Application = null;
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(permission);
                var results = permission.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Application: may not be null");
                Assert.IsTrue(permission.IsTransient());
                Assert.IsFalse(permission.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestPermissionWithNewApplicationDoesNotSave()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.Application = new Application();
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(permission);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.Permission.Application", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestPermissionSavesWithAnExistingApplication()
        {
            #region Arrange
            var permission = GetValid(9);
            var application = Repository.OfType<Application>().GetById(2);
            permission.Application = application;
            #endregion Arrange

            #region Act
            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.EnsurePersistent(permission);
            PermissionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(application, permission.Application);
            Assert.AreEqual("Name2", permission.Application.Name);
            Assert.IsFalse(permission.IsTransient());
            Assert.IsTrue(permission.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestPermissionWithUnsavedApplicationDoesNotSaveOrCascade()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.Application = CreateValidEntities.Application(99);
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(permission);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.Permission.Application", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestPermissionDeleteDoesNotCascadedToApplication()
        {
            #region Arrange
            var applicationCount = Repository.OfType<Application>().Queryable.Count();
            Assert.IsTrue(applicationCount > 0);
            var permissionCount = PermissionRepository.Queryable.Count();
            var permission = PermissionRepository.GetById(2);
            #endregion Arrange

            #region Act
            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.Remove(permission);
            PermissionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(applicationCount, Repository.OfType<Application>().Queryable.Count());
            Assert.AreEqual(permissionCount - 1, PermissionRepository.Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Application Tests

        #region Role Tests

        #region Invalid Tests

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPermissionWithNullRoleDoesNotSave()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.Role = null;
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(permission);
                var results = permission.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Role: may not be null");
                Assert.IsTrue(permission.IsTransient());
                Assert.IsFalse(permission.IsValid());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestPermissionWithNewRoleDoesNotSave()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.Role = new Role();
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(permission);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.Permission.Role", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestPermissionSavesWithAnExistingRole()
        {
            #region Arrange
            var permission = GetValid(9);
            var role = Repository.OfType<Role>().GetById(2);
            permission.Role = role;
            #endregion Arrange

            #region Act
            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.EnsurePersistent(permission);
            PermissionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(role, permission.Role);
            Assert.AreEqual("Name2", permission.Role.Name);
            Assert.IsFalse(permission.IsTransient());
            Assert.IsTrue(permission.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.PropertyValueException))]
        public void TestPermissionWithUnsavedRoleDoesNotSaveOrCascade()
        {
            Permission permission = null;
            try
            {
                #region Arrange
                permission = GetValid(9);
                permission.Role = CreateValidEntities.Role(99);
                #endregion Arrange

                #region Act
                PermissionRepository.DbContext.BeginTransaction();
                PermissionRepository.EnsurePersistent(permission);
                PermissionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(permission);
                Assert.IsNotNull(ex);
                Assert.AreEqual("not-null property references a null or transient value Catbert4.Core.Domain.Permission.Role", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestPermissionDeleteDoesNotCascadedToRole()
        {
            #region Arrange
            var roleCount = Repository.OfType<Role>().Queryable.Count();
            Assert.IsTrue(roleCount > 0);
            var permissionCount = PermissionRepository.Queryable.Count();
            var permission = PermissionRepository.GetById(2);
            #endregion Arrange

            #region Act
            PermissionRepository.DbContext.BeginTransaction();
            PermissionRepository.Remove(permission);
            PermissionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(roleCount, Repository.OfType<Role>().Queryable.Count());
            Assert.AreEqual(permissionCount - 1, PermissionRepository.Queryable.Count());
            #endregion Assert
        }
        #endregion Cascade Tests
        #endregion Role Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapPermission1()
        {
            #region Arrange
            var id = PermissionRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var role = Repository.OfType<Role>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Permission>(session, new PermissionEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.Role, role)
                .CheckProperty(c => c.User, user)
                .VerifyTheMappings();
            #endregion Act/Assert
        }


        [TestMethod]
        public void TestCanCorrectlyMapPermission2()
        {
            #region Arrange
            var id = PermissionRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var role = Repository.OfType<Role>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Permission>(session, new PermissionEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Inactive, false)
                .CheckProperty(c => c.Application, application)
                .CheckProperty(c => c.Role, role)
                .CheckProperty(c => c.User, user)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        /// <summary>
        /// Inactive hides record
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void TestCanCorrectlyMapPermission3()
        {
            #region Arrange
            var id = PermissionRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var application = Repository.OfType<Application>().GetById(2);
            var role = Repository.OfType<Role>().GetById(2);
            var user = Repository.OfType<User>().GetById(2);
            #endregion Arrange

            try
            {
                #region Act/Assert
                new PersistenceSpecification<Permission>(session, new PermissionEqualityComparer())
                    .CheckProperty(c => c.Id, id)
                    .CheckProperty(c => c.Inactive, true)
                    .CheckProperty(c => c.Application, application)
                    .CheckProperty(c => c.Role, role)
                    .CheckProperty(c => c.User, user)
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
            expectedFields.Add(new NameAndType("Application", "Catbert4.Core.Domain.Application", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Inactive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Role", "Catbert4.Core.Domain.Role", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("User", "Catbert4.Core.Domain.User", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Permission));

        }

        #endregion Reflection of Database.	
		
        public class PermissionEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is Application && y is Application)
                {
                    var xVal = (Application)x;
                    var yVal = (Application)y;
                    Assert.AreEqual(xVal.Name, yVal.Name);
                    Assert.AreEqual(xVal.Id, yVal.Id);
                    return true;
                }


                if (x is Role && y is Role)
                {
                    var xVal = (Role)x;
                    var yVal = (Role)y;
                    Assert.AreEqual(xVal.Name, yVal.Name);
                    Assert.AreEqual(xVal.Id, yVal.Id);
                    return true;
                }

                if (x is User && y is User)
                {
                    var xVal = (User)x;
                    var yVal = (User)y;
                    Assert.AreEqual(xVal.LoginId, yVal.LoginId);
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