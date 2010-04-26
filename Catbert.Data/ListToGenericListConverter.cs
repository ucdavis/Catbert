using System.Collections;
using System.Collections.Generic;

namespace CAESDO.Catbert.Data
{
    public class ListToGenericListConverter<T>
    {
        /// <summary>
        /// Converts a non-typed collection into a strongly typed collection.  This will fail if
        /// the non-typed collection contains anything that cannot be casted to type of T.
        /// </summary>
        /// <param name="listOfObjects">A <see cref="ICollection"/> of objects that will 
        /// be converted to a strongly typed collection.</param>
        /// <returns>Always returns a valid collection - never returns null.</returns>
        public List<T> ConvertToGenericList(IList listOfObjects) {
            ArrayList notStronglyTypedList = new ArrayList(listOfObjects);
            return new List<T>(notStronglyTypedList.ToArray(typeof(T)) as T[]);
        }
    }
}
