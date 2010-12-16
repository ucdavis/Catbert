using System.Collections.Generic;

namespace Catbert4.Models
{
    public class UserLookupModel
    {
        public IList<KeyValuePair<int,string>> Applications { get; set; }
        public IList<KeyValuePair<int, string>> Units { get; set; }
        public IList<KeyValuePair<int, string>> Roles { get; set; }
    }
}