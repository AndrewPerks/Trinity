using System.Collections.Generic;
using System.Security.Principal;
using Trinity.Model;

namespace Trinity.Authentication
{
    /// <summary>
    /// Handles user identities
    /// </summary>
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, string email, ICollection<Role> roles)
        {
            Name = name;
            Email = email;
            Roles = roles;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public ICollection<Role> Roles { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        #endregion
    }
}
