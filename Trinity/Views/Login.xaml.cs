using System.Windows;
using System.Windows.Controls;
using Trinity.Authentication;
using Trinity.ViewModels;

namespace Trinity.Views
{
    public interface IView
    {
        IViewModel ViewModel
        {
            get;
            set;
        }

        void Show();
    }

    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>        
    public partial class Login : Window, IView
    {
        public Login(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion

        private void TxtUsername_TextChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password.Length == 0)
            {
                BtnLogin.IsEnabled = false;
            }
            else
            {
                BtnLogin.IsEnabled = true;
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Cast the 'sender' to a PasswordBox
            PasswordBox pBox = sender as PasswordBox;
            //Set this "EncryptedPassword" dependency property to the "SecurePassword"
            //of the PasswordBox.
            PasswordBoxMvvmAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);             
            if (pBox.SecurePassword.Length <= 0)
            {
                BtnLogin.IsEnabled = false;
            }
            else
            {
                BtnLogin.IsEnabled = true;
            }
        }
    }
}
