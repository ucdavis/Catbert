using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CAESDO.Catbert.Core.Domain;

namespace CAESDO.Catbert.BLL
{
    [DataObject]
    public class MessageBLL : CAESArch.BLL.GenericBLLBase<Message, int>
    {
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void Insert(Message message)
        {
            EnsurePersistent(message);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<Message> GetActive()
        {
            return Queryable.Where(m => m.IsActive).OrderBy(m => m.MessageText);
        }
    }
}


