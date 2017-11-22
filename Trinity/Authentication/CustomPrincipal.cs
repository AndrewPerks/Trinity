using System.Security.Principal;

namespace Trinity.Authentication
{
    /// <summary>
    /// Check that a user identity has a matching role
    /// </summary>
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _identity;

        public CustomIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        public bool IsInRole(string selectedRole)
        {
            foreach (var role in _identity.Roles)
            {
                if (selectedRole == role.Role1)
                {
                    return role.Role1.Contains(selectedRole);                     
                }
            }
            return false;
        }

    }
}
