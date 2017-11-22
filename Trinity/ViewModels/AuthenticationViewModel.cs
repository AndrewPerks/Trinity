using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Threading;
using System.Windows;
using Trinity.Authentication;
using Trinity.Helpers;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Trinity.Views;
using Microsoft.Practices.Unity;

namespace Trinity.ViewModels
{
    public interface IViewModel { }

    /// <summary>
    /// Authentication view model which manages user authentication and the login stage
    /// </summary>
    public class AuthenticationViewModel : ValidatableBindableBase, IViewModel
    {
        #region Private Properties
        private string _username;
        private string _status;
        private SecureString _password;
        #endregion

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IContactService _contactService;

        #region Properties
        [Required(ErrorMessage = "Username is required")]
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        [Required(ErrorMessage = "Password is required")]
        public SecureString PasswordSecureString
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return string.Format("Signed in as {0}. {1}",
                          Thread.CurrentPrincipal.Identity.Name,
                          Thread.CurrentPrincipal.IsInRole("Administrator") ? "You are an administrator!"
                              : "You are NOT a member of the administrators group.");

                return "Not authenticated!";
            }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; this.OnPropertyChanged("Status"); }
        }
        #endregion

        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }

        public AuthenticationViewModel(IAuthenticationManager authenticationManager)
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _authenticationManager = authenticationManager;
            _contactService = ContainerHelper.Container.Resolve<ContactService>();
            
            LoadCommands();
        }

        private void LoadCommands()
        {
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
            LogoutCommand = new RelayCommand(OnLogout, CanLogout);
        }

        // User login
        private void OnLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || PasswordSecureString == null)
                {
                    Status = "You must enter username and password.";
                    return; 
                }

                // find user matching username
                var user = _contactService.GetByUsername(Username);
                if (user == null)
                {
                    Status = "Username and Password combination incorrect.";
                    return;
                }

                // User must enter new password
                if (user.PasswordHash == null && user.PasswordSalt == null)
                {
                    ChangePasswordViewModel viewModel = new ChangePasswordViewModel();
                    viewModel.Mode = Mode.Add;

                    var viewChangePassword = new ChangePassword();
                    viewChangePassword.DataContext = viewModel;
                    viewModel.Username = Username;
                    viewChangePassword.Show();

                    return;
                }

                if (!user.LoginActive || user.Deleted)
                {
                    Status = "Please contact the admin to enable the account.";
                    return;
                }

                if (user.FailedLoginCount >= 3)
                {
                    Status = "Maximum login attempts exceeded, please contact the administrator to reset your password.";
                    return;
                }

                var salt = user.PasswordSalt;

                foreach (char c in salt)
                {
                    PasswordSecureString.AppendChar(c);
                }

                // get hash of the entered data
                byte[] enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(this.PasswordSecureString));

                if (!PasswordHashing.SequenceEquals(enteredValueHash, user.PasswordHash))
                {
                    Status = "Username and Password combination incorrect";
                    user.FailedLoginCount += 1;
                    _contactService.UpdateContact(user);

                    return;
                }

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user    
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Email, user.Roles);

                // Reset failed login count
                user.FailedLoginCount = 0;
                _contactService.UpdateContact(user);

                // Out of hours check
                if (customPrincipal.Identity.IsAuthenticated && !customPrincipal.IsInRole("Administrator"))
                {
                    TimeSpan startTime = new TimeSpan(22, 0, 0); // Time the lockout should start
                    TimeSpan EndTime = new TimeSpan(07, 0, 0); // Time the lockout should end
                    DateTime currentDateTime = DateTime.Now;

                    var isOutOfHours = TimeManipulation.TimeBetween(currentDateTime, startTime, EndTime);                    
                    if (isOutOfHours)
                    {                        
                        OnLogout();
                        Status = "Users cannot login out of office hours.";
                        return;                    
                    }
                }

                //Update UI
                this.OnPropertyChanged("AuthenticatedUser");
                this.OnPropertyChanged("IsAuthenticated");
                LoginCommand.RaiseCanExecuteChanged();
                LogoutCommand.RaiseCanExecuteChanged();
                
                Username = string.Empty;                
                PasswordSecureString = null;
                Status = string.Empty;
               
                // Open main window
                var view = new MainWindow();
                view.Show();

                // Close current window
                var window = Application.Current.Windows[0];

                if (window != null)
                {
                    window.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                Status = "Login failed - Please provide some valid credentials.";
            }
            catch (Exception ex)
            {
                Status = string.Format("ERROR: {0}", ex.Message);
            }
        }

        // Checks the user login is valid
        private bool CanLogin()
        {
            if (String.IsNullOrWhiteSpace(Username) || HasErrors)
            {
                return false;
            }            

            return !IsAuthenticated;
        }

        // User logout
        private void OnLogout()
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null)
            {
                customPrincipal.Identity = new AnonymousIdentity();
                this.OnPropertyChanged("AuthenticatedUser");
                this.OnPropertyChanged("IsAuthenticated");
                LoginCommand.RaiseCanExecuteChanged();
                LogoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }

        // Checks user can logout 
        private bool CanLogout()
        {
            return IsAuthenticated;
        }

        // Checks user is authenticated
        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }
    }
}
