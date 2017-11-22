using System.ComponentModel;
using System.Threading;
using System.Windows;
using Trinity.Authentication;
using Trinity.Helpers;
using Trinity.Views;


namespace Trinity.ViewModels
{    
    public class MainWindowViewModel : BindableBase
    {        

        public RelayCommand<string> NavCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }
        public RelayCommand ContactsCommand { get; private set; }
        public RelayCommand ChangePasswordCommand { get; private set; }
        public RelayCommand UserCommand { get; private set; }
        public RelayCommand InactivityCommand { get; private set; }

        public MainWindowViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            LoadCommands();
        }

        private void LoadCommands()
        {
            LogoutCommand = new RelayCommand(OnLogout, CanLogout);
            ExitCommand = new RelayCommand(OnExit, CanExit);
            ContactsCommand = new RelayCommand(OpenContacts);
            ChangePasswordCommand = new RelayCommand(OpenChangePassword);
            UserCommand = new RelayCommand(OpenUsers);
            InactivityCommand = new RelayCommand(OpenInactivity); 
        }

        // Logout a user and redirect to login
        private void OnLogout()
        {
            //Application.Current.Windows
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
            }

            if (!(Application.Current.MainWindow is Login))
            {
                AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationManager());
                var loginWindow = new Login(viewModel);
                loginWindow.Show();

                foreach (var window in Application.Current.Windows)
                {
                    if (!(window is Login))
                    {
                        (window as Window).Close();
                    }                    
                }
            }
        }

        // Checks user can logout
        private bool CanLogout()
        {
            return true;
        }

        // Closes the application
        private void OnExit()
        {
            Application.Current.Shutdown();
        }

        // Checks that the application can close
        private bool CanExit()
        {
            return true;
        }

        // Open the client manager
        private void OpenContacts()
        {
            var view = new Clients();
            view.Show();
        }

        // Open the change password window
        private void OpenChangePassword()
        {
            var changePassword = new ChangePasswordViewModel();
            changePassword.Mode = Mode.Edit;

            var view = new ChangePassword();
            view.DataContext = changePassword;
            view.Show();
        }

        // Open the user manager
        private void OpenUsers()
        {
            var users = new Users();
            users.Show();
        }

        // Open the inactivity period window
        private void OpenInactivity()
        {
            var inactivityPeriod = new InactivityPeriod();
            inactivityPeriod.Show();
        }

    }
}
