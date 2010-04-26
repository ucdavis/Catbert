using System;
using System.Collections.Generic;
using NHibernate;
using CAESDO.Catbert.Core.DataInterfaces;
using NHibernate.Criterion;

namespace CAESDO.Catbert.Data
{
    public abstract class AbstractNHibernateDao<T, IdT> : IDao<T, IdT>
    {
        /// <summary>
        /// Loads an instance of type T from the DB based on its ID.
        /// </summary>
        public T GetById(IdT id, bool shouldLock)
        {
            T entity;

            if (shouldLock)
            {
                entity = (T)NHibernateSession.Load(persitentType, id, LockMode.Upgrade);
            }
            else
            {
                entity = (T)NHibernateSession.Load(persitentType, id);
            }

            return entity;
        }

        /// <summary>
        /// Gets the instance of type T from the DB based on its ID, OR NULL if the instance doesn't exist
        /// </summary>
        public T GetNullableByID(IdT id)
        {
            return NHibernateSession.Get<T>(id);
        }

        /// <summary>
        /// Loads every instance of the requested type with no filtering.
        /// </summary>
        public List<T> GetAll()
        {
            return GetByCriteria();
        }

        public List<T> GetAll(string propertyName, bool ascending)
        {
            return GetByCriteria(propertyName, ascending);
        }

        /// <summary>
        /// Loads every instance of the requested type using the supplied <see cref="ICriterion"></see>.
        /// If no <see cref="ICriterion"></see> is supplied, this behaves like <see cref="GetAll"></see>.
        /// </summary>
        public List<T> GetByCriteria(params ICriterion[] criterion)
        {
            string propertyName = String.Empty;
            bool ascending = false;

            return GetByCriteria(propertyName, ascending, criterion);
        }

        /// <summary>
        /// Overload that allows for sorting by one property
        /// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll" />.
        /// </summary>
        public List<T> GetByCriteria(string propertyName, bool ascending, params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }

            if (!string.IsNullOrEmpty(propertyName))
            {
                //If the propertyName is not empty, sort by that property
                criteria.AddOrder(new Order(propertyName, ascending));
            }

            ListToGenericListConverter<T> converter = new ListToGenericListConverter<T>();

            return converter.ConvertToGenericList(criteria.List());
        }

        public T GetByProperty(string propertyName, object propertyValue)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

            criteria.Add(Expression.Eq(propertyName, propertyValue));

            return criteria.UniqueResult<T>();
        }

        public List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            ListToGenericListConverter<T> converter = new ListToGenericListConverter<T>();
            return converter.ConvertToGenericList(criteria.List());
        }

        public List<T> GetByInclusionExample(T exampleInstance, string propertyName, bool ascending, params string[] propertiesToInclude)
        {
            //Get all of the properties of the given type (T)
            System.Reflection.PropertyInfo[] properties = persitentType.GetProperties();
            //System.Reflection.PropertyInfo[] properties = exampleInstance.GetType().GetProperties();

            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);
            Example example = Example.Create(exampleInstance);

            List<string> includedProperties = new List<string>(propertiesToInclude);

            //Exclude app properties except for the included properties
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (!includedProperties.Contains(property.Name))
                {
                    //only exclude the property if it isn't in the propertiesToInclude list
                    example.ExcludeProperty(property.Name);
                }
            }

            criteria.Add(example);

            if (!string.IsNullOrEmpty(propertyName))
            {
                criteria.AddOrder(new Order(propertyName, ascending));
            }

            ListToGenericListConverter<T> converter = new ListToGenericListConverter<T>();
            return converter.ConvertToGenericList(criteria.List());
        }

        public List<T> GetByInclusionExample(T exampleInstance, params string[] propertiesToInclude)
        {
            return this.GetByInclusionExample(exampleInstance, null, false, propertiesToInclude);
        }

        /// <summary>
        /// Looks for a single instance using the example provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            List<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count > 0)
            {
                return foundList[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// For entities that have assigned ID's, you must explicitly call Save to add a new one.
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        public T Save(T entity)
        {
            NHibernateSession.Save(entity);
            return entity;
        }

        /// <summary>
        /// For entities with automatatically generated IDs, such as identity, SaveOrUpdate may 
        /// be called when saving a new entity.  SaveOrUpdate can also be called to update any 
        /// entity, even if its ID is assigned.
        /// </summary>
        public T SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
        }

        /// <summary>
        /// Commits changes regardless of whether there's an open transaction or not
        /// </summary>
        public void CommitChanges()
        {
            if (NHibernateSessionManager.Instance.HasOpenTransaction())
            {
                NHibernateSessionManager.Instance.CommitTransaction();
            }
            else
            {
                // If there's no transaction, just flush the changes
                NHibernateSessionManager.Instance.GetSession().Flush();
            }
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        private ISession NHibernateSession
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSession();
            }
        }

        private Type persitentType = typeof(T);
    }
}
