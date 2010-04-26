using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CAESDO.Catbert.Test
{
    /// <summary>
    /// For better data integrity, it is imperitive that the <see cref="Entity.Id"/>
    /// property is read-only and set only by the ORM.  With that said, some unit tests need 
    /// Id set to a particular value; therefore, this utility enables that ability.  This class should 
    /// never be used outside of the testing project; instead, implement <see cref="IHasAssignedId" /> to 
    /// expose a public setter.
    /// </summary>
    public class EntityIdSetter
    {
        /// <summary>
        /// Uses reflection to set the Id of a <see cref="EntityWithTypedId" />.
        /// </summary>
        public static void SetIdOf<IdT>(object Entity, IdT id)
        {
            // Set the data property reflectively
            PropertyInfo idProperty = Entity.GetType().GetProperty("ID",
                                                                   BindingFlags.Public | BindingFlags.Instance);

            if (idProperty == null) throw new ArgumentException("idProperty could not be found");

            idProperty.SetValue(Entity, id, null);
        }
    }
}