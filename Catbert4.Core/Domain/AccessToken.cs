using System;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Catbert4.Core.Domain
{
    public class AccessToken : DomainObject
    {
        public AccessToken()
        {
            Active = true;
        }

        [Required]
        [Length(32, 32, Message = "Token must be 32 characters long")]
        public virtual string Token { get; set; }
               
        [Required]
        [Email]
        public virtual string ContactEmail { get; set; }
               
        public virtual string Reason { get; set; }

        public virtual bool Active { get; set; }
        
        [NotNull]
        public virtual Application Application { get; set; }

        /// <summary>
        /// Sets the token property to a new generated token
        /// </summary>
        public virtual void SetNewToken()
        {
            Token = GenerateToken();
        }

        /// <summary>
        /// Generates a new token
        /// </summary>
        /// <remarks>
        /// Just using a guid but without the dashes
        /// </remarks>
        /// <returns>Returns a unique string 32 chars long</returns>
        private static string GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
