using System.Collections.Generic;

namespace CAESDO.Catbert.Core.DataInterfaces
{
    public interface IDao<T, IdT>
    {
        T GetById(IdT id, bool shouldLock);
        T GetNullableByID(IdT id);
        List<T> GetAll();
        List<T> GetAll(string propertyName, bool ascending);
        List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
        List<T> GetByInclusionExample(T exampleInstance, params string[] propertiesToInclude);
        List<T> GetByInclusionExample(T exampleInstance, string propertyName, bool ascending, params string[] propertiesToInclude);
        T GetByProperty(string propertyName, object propertyValue);
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);
        void CommitChanges();
    }
}
