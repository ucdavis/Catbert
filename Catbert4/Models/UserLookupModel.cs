using System.Linq;

namespace Catbert4.Models
{
    public class UserLookupModel
    {
        public ILookup<int,string> Applications { get; set; }
        public ILookup<int,string> Units { get; set; }
        public ILookup<int,string> Roles { get; set; }
    }
}