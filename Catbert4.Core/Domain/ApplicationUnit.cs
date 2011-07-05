using UCDArch.Core.DomainModel;

namespace Catbert4.Core.Domain
{
    /// <summary>
    /// Simple class to bring together an app and associated untils
    /// </summary>
    public class ApplicationUnit : DomainObject
    {
        public virtual Application Application { get; set; }
        public virtual Unit Unit { get; set; }
    }
}