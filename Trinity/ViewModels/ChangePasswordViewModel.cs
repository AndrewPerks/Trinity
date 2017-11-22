using System;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Windows;
using Trinity.Authentication;
using Trinity.Helpers;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace Trinity.ViewModels
{
    /// <summary>
    /// Change the user password view model that allows existing or new passwords to be changed
    /// </summary>
    public class ChangePasswordViewModel : BindableBase
    {
        #region Private Properties
        private string _username;
        private SecureString _oldPassword;
        private SecureString _newPassword;
        private SecureString _confirmedPassword;
        #endregion

        private readonly IContactService _contactService;

        #region Properties
        public Mode Mode
        {
            get;
            set;
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public SecureString OldPasswordSecureString
        {
            get { return _oldPassword; }
            set { _oldPassword = value; this.OnPropertyChanged("OldPassword"); }
        }

        public SecureString NewPasswordSecureString
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }

        public SecureString ConfirmedPasswordSecureString
        {
            get { return _confirmedPassword; }
            set
            {
                _confirmedPassword = value;
                OnPropertyChanged("ConfirmedPassword");
            }
        }
        #endregion

        public RelayCommand<Window> SaveCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }

        public ChangePasswordViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _contactService = ContainerHelper.Container.Resolve<ContactService>();

            LoadData();
            LoadCommands();
        }

        private void LoadData()
        {
            Username = Thread.CurrentPrincipal.Identity.Name;
        }

        private void LoadCommands()
        {
            SaveCommand = new RelayCommand<Window>(UpdatePassword); // Add CanUpdatePassword to refactor
            CancelCommand = new RelayCommand<Window>(OnCancel);
        }

        // Updates the user password
        private void UpdatePassword(Window window)
        {
            try
            {
                if (this.Mode == Mode.Add)
                {
                    if (NewPasswordSecureString == null || ConfirmedPasswordSecureString == null)
                    {
                        MessageBox.Show("You must enter each password.");
                        return;
                    }
                }
                else if (this.Mode == Mode.Edit)
                {
                    if (OldPasswordSecureString == null || NewPasswordSecureString == null || ConfirmedPasswordSecureString == null)
                    {
                        MessageBox.Show("You must enter each password.");
                        return;
                    }
                }

                var user = _contactService.GetByUsername(Username);
                if (user == null)
                {
                    MessageBox.Show("User does not exist.");
                    return;
                }

                // If the user has no existing password
                if (this.Mode == Mode.Add)
                {
                    if (NewPasswordSecureString.Length < 6)
                    {
                        MessageBox.Show("Password must contain at least 6 characters.");
                        return;
                    }
                    var match = CheckNewPasswordAndConfirmationMatch(user);
                    if (match)
                    {
                        if (window != null)
                        {
                            window.Close();
                        }
                    }
                }
                // If the user is changing their existing password
                else if (this.Mode == Mode.Edit)
                {
                    if (NewPasswordSecureString.Length < 6)
                    {
                        MessageBox.Show("Password must contain at least 6 characters.");
                        return;
                    }
                    var match = CheckOldPasswordMatches(user);
                    if (match)
                    {
                        match = CheckNewPasswordAndConfirmationMatch(user);
                        if (match)
                        {
                            if (window != null)
                            {
                                window.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Old password does not match.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        private bool CheckOldPasswordMatches(Contact user)
        {
            // get the old password            
            var salt = user.PasswordSalt;

            foreach (char c in salt)
            {
                OldPasswordSecureString.AppendChar(c);
            }

            // get hash of the entered data
            byte[] enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(this.OldPasswordSecureString));

            if (!PasswordHashing.SequenceEquals(enteredValueHash, user.PasswordHash))
            {
                // Incorrect match
                return false;
            }
            return true;
        }

        private bool CheckNewPasswordAndConfirmationMatch(Contact user)
        {
            var newPassword = SecureStringManipulation.SecureStringToString(NewPasswordSecureString);
            var confirmedPassword = SecureStringManipulation.SecureStringToString(ConfirmedPasswordSecureString);

            if (newPassword == confirmedPassword)
            {
                var salt = PasswordHashing.GenerateSalt();

                foreach (char c in salt)
                {
                    NewPasswordSecureString.AppendChar(c);
                }

                var enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(this.NewPasswordSecureString));

                user.PasswordHash = enteredValueHash;
                user.PasswordSalt = salt;

                _contactService.UpdateContact(user);
                MessageBox.Show("User password updated.");
                return true;
            }
            else
            {
                // New and confirmed password dont match
                MessageBox.Show("Passwords do not match.");
                return false;
            }
        }

        // Close the window
        private void OnCancel(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
