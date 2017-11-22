using System.Security;
using System.Windows;

namespace Trinity.Authentication
{
    /// <summary>
    /// Attached property for the password box control
    /// </summary>
    public static class PasswordBoxMvvmAttachedProperties
    {
        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordProperty);
        }

        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for EncryptedPassword. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EncryptedPasswordProperty =
            DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(PasswordBoxMvvmAttachedProperties));
    }
}
