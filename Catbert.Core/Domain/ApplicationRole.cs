using CAESArch.Core.Domain;

namespace CAESDO.Catbert.Core.Domain
{
    /// <summary>
    /// Simple class to bring together an app, a role, and the role's hierarchy level in that application
    /// </summary>
    public class ApplicationRole : DomainObject<ApplicationRole, int>
    {
        public virtual Application Application { get; set; }
        public virtual Role Role { get; set; }

        public virtual int? Level { get; set; }
    }
}