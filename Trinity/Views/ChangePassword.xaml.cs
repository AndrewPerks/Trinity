using System.Windows;
using System.Windows.Controls;
using Trinity.Authentication;

namespace Trinity.Views
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>  
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Cast the 'sender' to a PasswordBox
            PasswordBox pBox = sender as PasswordBox;

            //Set this "EncryptedPassword" dependency property to the "SecurePassword"
            //of the PasswordBox.
            PasswordBoxMvvmAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }
    }

}
