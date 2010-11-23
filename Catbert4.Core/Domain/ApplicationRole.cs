using UCDArch.Core.DomainModel;

namespace Catbert4.Core.Domain
{
    /// <summary>
    /// Simple class to bring together an app, a role, and the role's hierarchy level in that application
    /// </summary>
    public class ApplicationRole : DomainObject
    {
        public virtual Application Application { get; set; }
        public virtual Role Role { get; set; }

        public virtual int? Level { get; set; }
    }
}