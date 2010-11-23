using UCDArch.Core.DomainModel;

namespace Catbert4.Core.Domain
{
    public class Permission : DomainObject
    {
        public virtual bool Inactive { get; set; }

        public virtual User User { get; set; }
        public virtual Application Application { get; set; }
        public virtual Role Role { get; set; }
    }
}