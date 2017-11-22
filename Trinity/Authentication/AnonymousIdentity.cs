namespace Trinity.Authentication
{
    /// <summary>
    /// Anonymous implementation of the user identity
    /// </summary>
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity(): base(string.Empty, string.Empty, null)
        { }
    }
}
