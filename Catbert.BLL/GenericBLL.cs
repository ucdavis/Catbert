using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CAESDO.Catbert.Core.DataInterfaces;
using CAESDO.Catbert.Core.Domain;
using CAESDO.Catbert.Data;
using System.Linq;

namespace CAESDO.Catbert.BLL
{
    [DataObject]
    public class GenericBLL<T, IdT>
    {
        public static IDaoFactory daoFactory
        {
            get
            {
                return new CAESDO.Catbert.Data.NHibernateDaoFactory();
            }
        }

        protected static IQueryable<T> Queryable
        {
            get
            {
                return daoFactory.GetGenericDao<T, IdT>().GetQueryable();
            }
        }

        public GenericBLL()
        {

        }

        public static T GetByID(IdT id)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetById(id, false);

                ts.CommittTransaction();

                return result;
            }
        }

        public static T GetNullableByID(IdT id)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetNullableByID(id);

                ts.CommittTransaction();

                return result;
            }
        }

        public static T GetByName(string name)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetByProperty("Name", name);

                ts.CommittTransaction();

                return result;
            }
        }

        public static T GetByProperty(string propertyName, object propertyValue)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetByProperty(propertyName, propertyValue);

                ts.CommittTransaction();

                return result;
            }
        }

        public static List<T> GetAll()
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetAll();

                ts.CommittTransaction();

                return result;
            }
        }

        public static List<T> GetAll(string propertyName, bool ascending)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetAll(propertyName, ascending);

                ts.CommittTransaction();

                return result;
            }
        }

        public static List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetByExample(exampleInstance, propertiesToExclude);

                ts.CommittTransaction();

                return result;
            }
        }

        public static T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetUniqueByExample(exampleInstance, propertiesToExclude);

                ts.CommittTransaction();

                return result;
            }
        }

        public static List<T> GetByInclusionExample(T exampleInstance, params string[] propertiesToInclude)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetByInclusionExample(exampleInstance, propertiesToInclude);

                ts.CommittTransaction();

                return result;
            }
        }

        public static List<T> GetByInclusionExample(T exampleInstance, string sortPropertyName, bool ascending, params string[] propertiesToInclude)
        {
            using (var ts = new TransactionScope())
            {
                var result = daoFactory.GetGenericDao<T, IdT>().GetByInclusionExample(exampleInstance, sortPropertyName, ascending, propertiesToInclude);

                ts.CommittTransaction();

                return result;
            }
        }

        public static bool MakePersistent(ref T entity)
        {
            //Don't force a save
            return MakePersistent(ref entity, false);
        }

        public static bool MakePersistent(ref T entity, bool forceSave)
        {
            //If the entity is of type domainObject, call validation prior to persisting
            if (entity is DomainObject<T, IdT>)
            {
                DomainObject<T, IdT> obj = entity as DomainObject<T, IdT>;

                if (obj.isValid(entity) == false)
                {
                    NHibernateSessionManager.Instance.RollbackTransaction();
                    return false;
                }
            }

            //Perform the requested operation
            if (forceSave)
            {
                entity = daoFactory.GetGenericDao<T, IdT>().Save(entity);
            }
            else
            {
                entity = daoFactory.GetGenericDao<T, IdT>().SaveOrUpdate(entity);
            }

            return true;
        }

        /// <summary>
        /// Like MakePersistent, but throws an application exception on persistance failure
        /// </summary>
        public static void EnsurePersistent(ref T entity)
        {
            EnsurePersistent(ref entity, false);
        }

        public static void EnsurePersistent(ref T entity, bool forceSave)
        {
            bool success = MakePersistent(ref entity, forceSave);

            if (!success)
            {
                string validationErrors = null;

                //If the entity is of type domainObject, get validation errors
                if (entity is DomainObject<T, IdT>)
                {
                    DomainObject<T, IdT> obj = entity as DomainObject<T, IdT>;

                    validationErrors = obj.getValidationResultsAsString(entity);
                }

                StringBuilder errorMessage = new StringBuilder();

                errorMessage.AppendLine(string.Format("Object of type {0} could not be persisted\n\n", typeof(T)));

                if (!string.IsNullOrEmpty(validationErrors))
                {
                    errorMessage.Append("Validation Errors: ");
                    errorMessage.Append(validationErrors);
                }

                throw new ApplicationException(errorMessage.ToString());
            }
        }

        public static void Remove(T entity)
        {
            daoFactory.GetGenericDao<T, IdT>().Delete(entity);
        }

        /// <summary>
        /// Calls the remove method on all entities in the given list collection
        /// </summary>
        /// <remarks>Could probably abstract out to IEnumerable later</remarks>
        public static void Remove(List<T> entities)
        {
            foreach (T entity in entities)
            {
                Remove(entity);
            }
        }
    }
}
